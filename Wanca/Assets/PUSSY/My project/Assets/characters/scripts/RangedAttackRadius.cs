using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttackRadius : AttackRange
{
    public NavMeshAgent Agent;
    public Bullet BulletPrefab;
    public Vector3 BulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask Mask;
    private ObjectPool BulletPool;
    [SerializeField]
    private float SpherecastRadius = 0.1f;
    private RaycastHit Hit;
    private IDamageable targetDamageable;
    private Bullet bullet;
    private bool canShoot = false; // Nuevo: bandera para indicar si puede disparar

    protected override void Awake()
    {
        base.Awake();
        BulletPool = ObjectPool.CreateInstance(BulletPrefab, Mathf.CeilToInt((1 / AttackDelay) * BulletPrefab.AutoDestroyTime));
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        while (Damageables.Count > 0)
        {
            for (int i = 0; i < Damageables.Count; i++)
            {
                // Detectar si tiene línea de visión con el objetivo
                if (HasLineOfSightTo(Damageables[i].GetTransform()))
                {
                    targetDamageable = Damageables[i];
                    OnAttack?.Invoke(Damageables[i]);

                    // Detener el agente si puede disparar
                    Agent.isStopped = true;
                    canShoot = true; // Puede disparar
                    break;
                }
            }

            // Si se puede disparar
            if (canShoot && targetDamageable != null)
            {
                // Obtener la bala del pool y disparar
                PoolableObject poolableObject = BulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<Bullet>();

                    bullet.Damage = Damage;
                    bullet.transform.position = transform.position + BulletSpawnOffset;
                    bullet.transform.rotation = Agent.transform.rotation;
                    bullet.Rigidbody.AddForce(Agent.transform.forward * BulletPrefab.MoveSpeed, ForceMode.VelocityChange);
                }
            }
            else
            {
                // Si no se puede disparar, seguir moviéndose
                Agent.isStopped = false;
            }

            yield return Wait;

            // Si el objetivo se pierde o no tiene línea de visión, el agente sigue moviéndose
            if (targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                canShoot = false; // Ya no puede disparar
                Agent.isStopped = false;
            }

            Damageables.RemoveAll(DisabledDamageables);
        }

        // Reiniciar el agente al terminar el ataque
        Agent.isStopped = false;
        AttackCoroutine = null;
    }

    private bool HasLineOfSightTo(Transform Target)
    {
        // Usar Raycast para verificar la línea de visión
        if (Physics.Raycast(transform.position + BulletSpawnOffset, (Target.position - transform.position).normalized, out Hit, Collider.radius, Mask))
        {
            IDamageable damageable;
            if (Hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == Target; // Solo si el objetivo es el mismo que queremos atacar
            }
        }
        return false;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (AttackCoroutine == null)
        {
            Agent.isStopped = false;
        }
    }
}
