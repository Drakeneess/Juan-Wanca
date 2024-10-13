using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    public Sprite[] sprites; // Asegúrate de que este array contenga los sprites correspondientes
    private Image image;
    private Item item;
    private ItemController itemController;
    private string inputCharacter;
    private InputCharacter input;
    private float rotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Iniciar una coroutine para retrasar la búsqueda del item por un frame
        StartCoroutine(FindItemNextFrame());

        image = GetComponentInChildren<Image>();
        input = FindObjectOfType<InputCharacter>();

        // Establecer el sprite según el esquema de entrada actual
        image.gameObject.SetActive(false);

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
        if (item != null)
        {
            if (item.IsPickedUp())
            {
                Destroy(gameObject);
            }
            else
            {
                // Rotar el item
                transform.Rotate(Vector3.up, rotationSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemController = other.GetComponent<ItemController>();
            if (itemController != null)
            {
                itemController.SetItemAwayState(false);
                itemController.GetItem(item);

                image.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (itemController != null)
            {
                itemController.SetItemAwayState(true);
                itemController.RemoveItem();
                itemController = null;

                image.gameObject.SetActive(false);
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

        print(inputCharacter);
    }
}
