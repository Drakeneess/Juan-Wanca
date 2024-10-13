using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class EnemyMortar : MonoBehaviour, IDamageable
{
    public Transform player; // El jugador
    public GameObject mortarProjectilePrefab; // Proyectil del mortero (esfera)
    public Transform firePoint; // Punto desde donde disparar� el proyectil
    public GameObject impactMarkerPrefab; // Marcador de impacto
    public float mortarFireDelay = 2f; // Tiempo entre disparos
    public float fireRange = 20f; // Rango de disparo
    public int health = 100; // Salud del enemigo
    private NavMeshAgent agent; // Para el movimiento en el NavMesh
    private Animator animator;

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("No se pudo encontrar el Animator en los hijos del objeto.");
        }

        StartCoroutine(FireMortar()); // Empezar a disparar morteros
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= fireRange)
        {
            // Girar hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // No cambiar la rotaci�n en Y
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    // M�todo para disparar proyectiles de mortero
    // M�todo para disparar proyectiles de mortero
    private IEnumerator FireMortar()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= fireRange)
            {
                // Activar animaci�n de ataque
                animator.SetTrigger("Attack");

                // Crear proyectil de mortero
                GameObject mortarProjectile = Instantiate(mortarProjectilePrefab, firePoint.position, Quaternion.identity);

                // Obtener la direcci�n hacia el jugador
                Vector3 direction = (player.position - firePoint.position).normalized;

                // Iniciar la trayectoria del proyectil sin usar f�sica
                StartCoroutine(MoveProjectile(mortarProjectile, direction, player.position));

                // Crear el marcador en el lugar donde caer� la bala
                GameObject impactMarker = Instantiate(impactMarkerPrefab, player.position, Quaternion.identity);
                Destroy(impactMarker, 2f); // Destruir el marcador despu�s de 2 segundos
            }
            yield return new WaitForSeconds(mortarFireDelay); // Esperar antes de disparar de nuevo
        }
    }

    // M�todo para mover el proyectil
    private IEnumerator MoveProjectile(GameObject projectile, Vector3 direction, Vector3 targetPosition)
    {
        float projectileSpeed = 5f; // Ajusta la velocidad seg�n lo necesario
        float travelTime = 1f; // Tiempo de vuelo hasta el objetivo

        float elapsedTime = 0f;
        Vector3 startPosition = projectile.transform.position;

        while (elapsedTime < travelTime)
        {
            // Calcula la posici�n del proyectil a lo largo del tiempo
            float t = elapsedTime / travelTime;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            newPosition.y += Mathf.Sin(t * Mathf.PI) * 6; // Ajustar altura para efecto de arco
            projectile.transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null; // Esperar un frame
        }

        // Aseg�rate de que el proyectil llegue al destino final
        projectile.transform.position = targetPosition;

        // Destruir el proyectil despu�s de llegar
        Destroy(projectile);
    }

    // Funciones para manejar el da�o (interfaz IDamageable)
    public void TakeDamage(int Damage)
    {
        health -= Damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Escape(); // Si recibe da�o, se mueve a otro lugar
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void Die()
    {
        // Destruir el enemigo o activar animaciones de muerte
        gameObject.SetActive(false);

    }

    private void Escape()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f; // Distancia para escapar
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);
        animator.SetBool("corre", true); // Activar animaci�n de correr
    }
}

