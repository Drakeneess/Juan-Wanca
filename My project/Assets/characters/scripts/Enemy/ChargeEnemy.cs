using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargingEnemy : MonoBehaviour, IDamageable
{
    public float chargeDistance = 10f;   // Distancia que recorrer� el enemigo durante la embestida
    public float chargeSpeed = 5f;       // Velocidad de embestida
    public float idleTime = 2f;          // Tiempo que esperar� entre embestidas
    public int chargeDamage = 50;        // Da�o de la embestida
    public GameObject chargeIndicatorPrefab;   // Prefab del indicador de embestida
    public int health = 100;             // Vida del enemigo

    private Transform player;            // Referencia al jugador
    private bool isCharging = false;     // Estado de embestida
    private Animator animator;           // Controlador de animaciones
    private NavMeshAgent agent;          // Agente para navegaci�n del enemigo
    private bool isRam = false;          // Estado de animaci�n embestida
    private GameObject currentIndicator; // Referencia al indicador de embestida

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        while (true)
        {
            // Fase de carga: mostrar el indicador y preparar el ataque
            ShowChargeIndicator();
            RotateTowardsPlayer();
            isRam = false; // Estado idle

            if (animator != null)
            {
                animator.SetBool("isRam", false); // Cambiar a estado idle en animaci�n
            }

            yield return new WaitForSeconds(idleTime); // Esperar antes de embestir

            // Fase de embestida
            isCharging = true;
            isRam = true;

            if (animator != null)
            {
                animator.SetBool("isRam", true); // Cambiar a estado de embestida en animaci�n
            }

            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 targetPosition = transform.position + direction * chargeDistance;

            float elapsedTime = 0f;
            while (elapsedTime < chargeDistance / chargeSpeed)
            {
                agent.Move(direction * chargeSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;

                // Si colisiona con el jugador
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(chargeDamage);
                            Debug.Log("El jugador ha recibido " + chargeDamage + " de da�o por la embestida.");
                        }
                    }
                }

                yield return null; // Esperar al siguiente frame
            }

            // Destruir el indicador despu�s de la embestida
            if (currentIndicator != null)
            {
                Destroy(currentIndicator);
            }

            isCharging = false; // Terminar embestida

            yield return new WaitForSeconds(idleTime); // Esperar antes de la siguiente embestida
        }
    }

    void ShowChargeIndicator()
    {
        // Destruir cualquier indicador anterior que a�n est� presente
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        // Crear un nuevo indicador en la posici�n del enemigo
        currentIndicator = Instantiate(chargeIndicatorPrefab, transform.position, Quaternion.identity);

        // Calcular la direcci�n de la embestida (hacia el jugador)
        Vector3 direction = (player.position - transform.position).normalized;

        // Calcular el punto final de la embestida
        Vector3 targetPosition = transform.position + direction * chargeDistance;

        // Orientar el indicador en la direcci�n de la embestida
        currentIndicator.transform.forward = direction;

        // Calcular la distancia entre el enemigo y el punto de embestida
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Ajustar la escala del indicador en el eje Z (profundidad o largo) para que coincida con la distancia
        currentIndicator.transform.localScale = new Vector3(currentIndicator.transform.localScale.x, currentIndicator.transform.localScale.y, distance);

        // Comenzar la rotaci�n del indicador
        StartCoroutine(UpdateIndicatorRotation());
    }

    private IEnumerator UpdateIndicatorRotation()
    {
        // Continuamente actualizar la rotaci�n del indicador para mirar al jugador mientras est� en la fase de carga
        while (!isCharging)
        {
            if (currentIndicator == null)
            {
                yield break; // Salir si el indicador fue destruido
            }

            // Calcular la direcci�n hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            // Actualizar la rotaci�n del indicador para que apunte al jugador
            currentIndicator.transform.forward = direction;

            // Rotar el enemigo hacia el jugador
            RotateTowardsPlayer();

            yield return null; // Esperar hasta el siguiente frame
        }
    }

    void RotateTowardsPlayer()
    {
        // Calcular la direcci�n hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotar suavemente hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        if (!isCharging)
        {
            health -= damage;
            Debug.Log("El enemigo ha recibido " + damage + " de da�o. Salud restante: " + health);

            if (health <= 0)
            {
                Die();
            }
        }
        else
        {
            Debug.Log("El enemigo es invulnerable durante la embestida.");
        }
    }

    private void Die()
    {
        Debug.Log("El enemigo ha muerto.");
        gameObject.SetActive(false);
    }

    // Implementaci�n de GetTransform para la interfaz IDamageable
    public Transform GetTransform()
    {
        return this.transform;
    }
}

