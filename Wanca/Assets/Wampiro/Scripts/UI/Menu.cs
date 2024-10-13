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

    void Update()
    {
        // Puedes agregar más lógica si es necesario
    }

    private void OnDisable()
    {
        inputs.Menu.Disable();
    }
}
