using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float damage = 1f; // Daño que inflige el proyectil
    public float lifeTime = 2f; // Tiempo de vida del proyectil
    public LayerMask enemyLayer; // Capa que determina qué objetos son enemigos

    private float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        // Destruir el proyectil después de que su tiempo de vida haya expirado
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Detectar colisión con enemigos
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto con el que colisionamos está en la capa de enemigos
        if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0 && other.gameObject.CompareTag("Enemy"))
        {
            // Obtener el controlador del enemigo y aplicar daño
            CharacterGeneralController enemy = other.gameObject.GetComponent<CharacterGeneralController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destruir el proyectil tras la colisión
            Destroy(gameObject);
        }
    }
}
