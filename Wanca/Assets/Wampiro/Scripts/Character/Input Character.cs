using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCharacter : MonoBehaviour
{
    private InputActions inputs;
    private MovingCharacter movingCharacter;
    private CharacterRotation characterRotation;
    private AttackController attackController;
    private Tackle tackle;

    private Vector2 directionCharacter;
    private Vector2 viewDirectionCharacter;
    private string currentScheme = "Mouse & Keyboard";

    // Start is called before the first frame update
    void Awake()
    {
        inputs = new InputActions();
        movingCharacter = GetComponent<MovingCharacter>();
        characterRotation = GetComponent<CharacterRotation>();
        attackController = GetComponent<AttackController>();
        tackle = GetComponent<Tackle>();

        // Subscribirse a los cambios de esquema de control
        InputSystem.onActionChange += OnInputActionChange;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        RotateCharacter();
        Attack();
        Tackle();
    }

    void OnEnable()
    {
        inputs.Game.Enable();
    }

    void OnDisable()
    {
        inputs.Game.Disable();
    }

    private void MoveCharacter()
    {
        directionCharacter = inputs.Game.Movement.ReadValue<Vector2>();
        movingCharacter.MoveCharacter(directionCharacter);
    }

    private void RotateCharacter()
    {
        if (currentScheme == "Gamepad")
        {
            // Rotación usando un stick del gamepad
            viewDirectionCharacter = inputs.Game.Aim.ReadValue<Vector2>();

            characterRotation.RotateCharacter(viewDirectionCharacter);
        }
        else if (currentScheme == "Mouse & Keyboard")
        {
            // Obtenemos la posición del mouse en pantalla
            Vector3 mouseScreenPosition = inputs.Game.AimMouse.ReadValue<Vector2>();
            // Seguir el movimiento del mouse
            characterRotation.RotateCharacterWithMouse(mouseScreenPosition);
        }
    }
    private void Attack(){
        if(inputs.Game.Attack.triggered){
            attackController.Attack();
        }
    }
    private void Tackle(){
        if(inputs.Game.Tackle.triggered){
            tackle.TackleAttack();
        }
    }

     // Detectar cambios en el esquema de control
    private void OnInputActionChange(object obj, InputActionChange change)
    {
        if (obj is InputAction action && change == InputActionChange.ActionPerformed)
        {
            var device = action.activeControl.device;

            // Ignorar movimientos pequeños del mouse
            if (device is Mouse && action.name == "Aim Mouse") 
            {
                return;  // Evitar cambios de esquema si solo es movimiento del mouse.
            }

            // Detectar el dispositivo activo y cambiar el esquema
            if (device is Keyboard || device is Mouse)
            {
                currentScheme = "Mouse & Keyboard";
                Cursor.lockState = CursorLockMode.None;  // Cursor desbloqueado para el mouse.
                Cursor.visible = true;
            }
            else if (device is Gamepad)
            {
                currentScheme = "Gamepad";
                Cursor.lockState = CursorLockMode.Locked;  // Cursor bloqueado para el gamepad.
                Cursor.visible = false;
            }
            else
            {
                currentScheme = device.displayName;
            }
        }
    }
}
