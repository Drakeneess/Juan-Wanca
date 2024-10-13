using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public enum MenuType
    {
        vertical,
        horizontal,
        matrix
    }
    public MenuType menuType;

    protected Button[] buttons;
    protected int currentSelection = 0; // Índice del botón seleccionado
    protected InputActions inputs;
    protected Vector2 cameraDirection;

    public float navigationDelay = 0.2f; // Tiempo de delay entre navegaciones
    private float navigationCooldown = 0f; // Temporizador para controlar el delay
    private int columns = 3;  // Número de columnas en el menú de tipo matriz

    protected virtual void Start()
    {
        inputs = new InputActions();
        inputs.Menu.Enable();

        buttons = GetComponentsInChildren<Button>();

        // Asignar eventos para el mouse
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            EventTrigger trigger = buttons[i].gameObject.AddComponent<EventTrigger>();

            // Crear la entrada de evento OnPointerEnter
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnMouseHover(index); });
            trigger.triggers.Add(entry);
        }

        // Inicializar la selección
        UpdateButtonSelection();

        // Asignar las acciones de entrada
        inputs.Menu.Navigate.performed += ctx => Navigate(ctx.ReadValue<Vector2>());
        inputs.Menu.Select.performed += ctx => Select();
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            // Resaltar el botón seleccionado
            buttons[i].Select();
            buttons[i].interactable = i == currentSelection; // Solo el botón seleccionado es interactuable
        }
    }

    private void Navigate(Vector2 direction)
    {
        // Solo navegar si el tiempo de espera ha pasado
        if (navigationCooldown <= 0f)
        {
            switch (menuType)
            {
                case MenuType.vertical:
                    NavigateVertical(direction);
                    break;

                case MenuType.horizontal:
                    NavigateHorizontal(direction);
                    break;

                case MenuType.matrix:
                    NavigateMatrix(direction);
                    break;
            }

            // Reiniciar el temporizador de cooldown
            navigationCooldown = navigationDelay;
        }
    }

    private void NavigateVertical(Vector2 direction)
    {
        if (direction.y > 0) // Arriba
        {
            currentSelection--;
        }
        else if (direction.y < 0) // Abajo
        {
            currentSelection++;
        }

        // Asegurarse de que la selección esté dentro del rango
        currentSelection = Mathf.Clamp(currentSelection, 0, buttons.Length - 1);
        UpdateButtonSelection();
    }

    private void NavigateHorizontal(Vector2 direction)
    {
        if (direction.x > 0) // Derecha
        {
            currentSelection++;
        }
        else if (direction.x < 0) // Izquierda
        {
            currentSelection--;
        }

        // Asegurarse de que la selección esté dentro del rango
        currentSelection = Mathf.Clamp(currentSelection, 0, buttons.Length - 1);
        UpdateButtonSelection();
    }

    private void NavigateMatrix(Vector2 direction)
    {
        int rows = Mathf.CeilToInt((float)buttons.Length / columns); // Calcular cuántas filas tiene el menú

        if (direction.y > 0) // Arriba
        {
            currentSelection -= columns; // Moverse a la fila anterior
        }
        else if (direction.y < 0) // Abajo
        {
            currentSelection += columns; // Moverse a la fila siguiente
        }
        else if (direction.x > 0) // Derecha
        {
            currentSelection++;
        }
        else if (direction.x < 0) // Izquierda
        {
            currentSelection--;
        }

        // Asegurarse de que la selección esté dentro del rango
        currentSelection = Mathf.Clamp(currentSelection, 0, buttons.Length - 1);
        UpdateButtonSelection();
    }

    private void Select()
    {
        // Ejecutar la acción del botón seleccionado
        if (currentSelection >= 0 && currentSelection < buttons.Length)
        {
            buttons[currentSelection].onClick.Invoke();
        }
    }

    // Llamada cuando el mouse pasa por encima de un botón
    public void OnMouseHover(int buttonIndex)
    {
        currentSelection = buttonIndex;
        UpdateButtonSelection();
    }

    protected virtual void Update()
    {
        // Reducir el cooldown del delay
        if (navigationCooldown > 0f)
        {
            navigationCooldown -= Time.deltaTime;
        }

        // Controlar el movimiento de la cámara si es necesario
        CameraMovement();
    }

    private void OnDisable()
    {
        inputs.Menu.Disable();
    }

    public Vector2 GetCameraMovement()
    {
        return cameraDirection;
    }

    private void CameraMovement()
    {
        cameraDirection = inputs.Menu.Camera.ReadValue<Vector2>();
    }
}
