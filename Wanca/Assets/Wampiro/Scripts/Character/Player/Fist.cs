using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    private Collider fistCollider;

    // Daño que inflige el puño
    public int punchDamage = 1;

    void Start()
    {
        // Obtiene el Collider del puño y lo desactiva inicialmente
        fistCollider = GetComponent<Collider>();
        if (fistCollider == null)
        {
            Debug.LogError("No se encontró un Collider en el puño.");
        }
        fistCollider.enabled = false;  // Desactiva el Collider al inicio
    }

    // Método para activar o desactivar el Collider
    public void EnableCollider(bool enable)
    {
        if (fistCollider != null)
        {
            fistCollider.enabled = enable;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto con el que colisiona es un enemigo
        if (other.CompareTag("Enemy"))
        {
            // Obtiene el componente de salud del enemigo y aplica el daño
            CharacterGeneralController enemyHealth = other.GetComponent<CharacterGeneralController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(punchDamage);
            }
        }
    }
}
