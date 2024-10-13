using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    public Sprite[] sprites; // Asegúrate de que este array contenga los sprites correspondientes
    public float interactDistance = 5f; // Distancia máxima para interactuar
    private Image image;
    private Item item;
    private ItemController itemController;
    private string inputCharacter;
    private InputCharacter input;
    private GameObject player;
    private float rotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Iniciar una coroutine para retrasar la búsqueda del item por un frame
        StartCoroutine(FindItemNextFrame());

        image = GetComponentInChildren<Image>();
        input = FindObjectOfType<InputCharacter>();
        player = GameObject.FindGameObjectWithTag("Player"); // Encuentra al jugador
        image.gameObject.SetActive(false); // Desactivar la imagen al inicio
    }

    // Coroutine para esperar un frame antes de buscar el item
    IEnumerator FindItemNextFrame()
    {
        yield return null;  // Espera un frame
        item = GetComponentInChildren<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        SetSpriteAccordingToInput();
        HandleItemInteraction();
        if (item != null && !item.IsPickedUp())
        {
            // Rotar el item
            transform.Rotate(Vector3.up, rotationSpeed);
        }
    }

    // Método para manejar la interacción del ítem según la distancia al jugador
    private void HandleItemInteraction()
    {
        if (player != null && item != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= interactDistance)
            {
                if (itemController == null)
                {
                    itemController = player.GetComponent<ItemController>();
                }

                if (itemController != null)
                {
                    itemController.SetItemAwayState(false);
                    itemController.GetItem(item);
                    image.gameObject.SetActive(true); // Mostrar la imagen cuando el jugador está en rango

                    // Si el ítem es recogido, destruir el GameObject
                    if (item.IsPickedUp())
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (itemController != null)
                {
                    itemController.SetItemAwayState(true);
                    itemController.RemoveItem();
                    itemController = null;
                }

                image.gameObject.SetActive(false); // Ocultar la imagen cuando el jugador está fuera de rango
            }
        }
    }

    // Método para establecer el sprite según el esquema de entrada
    private void SetSpriteAccordingToInput()
    {
        inputCharacter = input.CurrentScheme;

        if (sprites.Length > 0)
        {
            // Asumiendo que el índice 0 es para Mouse & Keyboard y el índice 1 es para Gamepad
            if (inputCharacter == "Mouse & Keyboard")
            {
                image.sprite = sprites[0]; // Cambia al sprite correspondiente para Mouse & Keyboard
            }
            else if (inputCharacter == "Gamepad")
            {
                image.sprite = sprites[1]; // Cambia al sprite correspondiente para Gamepad
            }
        }
    }
}
