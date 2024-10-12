using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : PoolableObject, IDamageable
{
    public AttackRange AttackRadius;
    public Animator Animator;
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public EnemyBehaviurs EnemyScriptableObject;
    public int Health = 100;

    public GameObject head;
    public GameObject rodillaIzq;
    public GameObject rodillaDere;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public float fadeDuration = 4f;

    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";

    private void Awake()
    {
        AttackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(IDamageable Target)
    {
        Animator.SetTrigger(ATTACK_TRIGGER);

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Agent.enabled = false;
    }

    public virtual void SetupAgentFromConfiguration()
    {
        Agent.acceleration = EnemyScriptableObject.Acceleration;
        Agent.angularSpeed = EnemyScriptableObject.AngularSpeed;
        Agent.areaMask = EnemyScriptableObject.AreaMask;
        Agent.avoidancePriority = EnemyScriptableObject.AvoidancePriority;
        Agent.baseOffset = EnemyScriptableObject.BaseOffset;
        Agent.height = EnemyScriptableObject.Height;
        Agent.obstacleAvoidanceType = EnemyScriptableObject.ObstacleAvoidanceType;
        Agent.radius = EnemyScriptableObject.Radius;
        Agent.speed = EnemyScriptableObject.Speed;
        Agent.stoppingDistance = EnemyScriptableObject.StoppingDistance;

        Movement.UpdateRate = EnemyScriptableObject.AIUpdateInterval;
        Health = EnemyScriptableObject.Health;

        (AttackRadius.Collider == null ? AttackRadius.GetComponent<SphereCollider>() : AttackRadius.Collider).radius = EnemyScriptableObject.AttackRadius;
        AttackRadius.AttackDelay = EnemyScriptableObject.AttackDelay;
        AttackRadius.Damage = EnemyScriptableObject.Damage;
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Desacoplar las partes del cuerpo
        head.transform.parent = null;
        rodillaIzq.transform.parent = null;
        rodillaDere.transform.parent = null;

        // Añadir Rigidbody para las físicas de cada parte
        Rigidbody headRb = head.AddComponent<Rigidbody>();
        Rigidbody rodillaIzqRb = rodillaIzq.AddComponent<Rigidbody>();
        Rigidbody rodillaDereRb = rodillaDere.AddComponent<Rigidbody>();

        // Aplicar fuerza de explosión a cada parte del cuerpo
        headRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        rodillaIzqRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        rodillaDereRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

        // Iniciar desvanecimiento y destrucción después de 4 segundos
        StartCoroutine(FadeAndDestroy(head));
        StartCoroutine(FadeAndDestroy(rodillaIzq));
        StartCoroutine(FadeAndDestroy(rodillaDere));

        // Finalmente, desactivar el enemigo
        gameObject.SetActive(false);
    }

    private IEnumerator FadeAndDestroy(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            renderer.material.color = newColor;

            yield return null;
        }

        Destroy(obj); // Destruye la parte del cuerpo después del desvanecimiento
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

