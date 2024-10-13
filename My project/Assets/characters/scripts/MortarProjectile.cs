using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MortarProjectile : MonoBehaviour
{
    public float AutoDestroyTime = 5f; // Tiempo antes de que el proyectil se destruya
    public float MoveSpeed = 2f; // Velocidad de movimiento
    public int Damage = 25; // Da�o que infligir� el proyectil
    public Vector3 launchOffset; // Offset configurable para el lanzamiento


    private Rigidbody rb; // Componente Rigidbody del proyectil

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Establecer la posici�n inicial del proyectil con el offset
        transform.position += launchOffset;

        // Iniciar el movimiento del proyectil hacia arriba
        StartCoroutine(MoveProjectile());
        // Programar la destrucci�n autom�tica
        Invoke(nameof(Disable), AutoDestroyTime);
    }

    private IEnumerator MoveProjectile()
    {
        // Mover el proyectil hacia arriba
        Vector3 targetPosition = transform.position + Vector3.up * 10f; // Cambia la altura si es necesario

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Mover el proyectil hacia arriba
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);

            // Verificar colisi�n con el jugador usando Raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f)) // Ajusta la distancia seg�n sea necesario
            {
                if (hit.collider.CompareTag("Player"))
                {
                    IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(Damage);
                        Debug.Log($"{hit.collider.name} ha recibido {Damage} de da�o.");
                    }
                    Disable(); // Destruir el proyectil
                    yield break; // Salir del coroutine
                }
            }

            yield return null; // Esperar un frame
        }

        // Comenzar a caer despu�s de alcanzar la altura
        while (transform.position.y > 0)
        {
            transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
            yield return null; // Esperar un frame
        }

        // Destruir el proyectil si alcanza el suelo
        Disable();
    }

    private void Disable()
    {
        CancelInvoke(); // Cancelar cualquier llamada a Disable
        rb.velocity = Vector3.zero; // Detener el movimiento
        gameObject.SetActive(false); // Desactivar el proyectil
    }
}









