using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    // Definimos el tipo de consumible
    public enum ConsumableType
    {
        Health,
        Poison // Notar que la palabra correcta es "Poison" (veneno), no "Poision"
    }

    // Variable para almacenar el tipo de consumible
    public ConsumableType consumableType;

    // Start is called before the first frame update
    void Start()
    {
        // Inicialización, si es necesario
    }

    // Update is called once per frame
    void Update()
    {
        // Lógica de actualización, si es necesario
    }

    // Override de la acción del objeto consumible
    public override void Action(Transform player)
    {
        // Llamamos al método base si hay lógica heredada que queremos mantener
        base.Action(player);

        // Accedemos al controlador general del jugador
        PlayerGeneralController playerController = player.GetComponent<PlayerGeneralController>();
        if (playerController != null) 
        {
            switch (consumableType)
            {
                case ConsumableType.Health:
                    // Llamamos a la función de curación del jugador
                    playerController.Cure();
                    break;

                case ConsumableType.Poison:
                    // Aplicamos daño al jugador (en este caso, 1 de daño)
                    playerController.TakeDamage(1);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("PlayerGeneralController no encontrado en el jugador.");
        }
    }
}
