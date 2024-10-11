using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    private bool canRotate=true;
    // Update is called once per frame
    void Update()
    {
        // Aquí podrías manejar otras lógicas de actualización si es necesario.
    }

    // Método que rota el personaje basado en la dirección del joystick
    public void RotateCharacter(Vector2 joystickInput)
    {
        if(canRotate){
            // Asegurarse de que hay entrada significativa del joystick
            if (joystickInput.magnitude >= 0.1f)
            {
                // Convertir el vector de dirección a un vector 3D
                Vector3 targetDirection = new Vector3(-joystickInput.x, 0, -joystickInput.y);

                // Calcular la rotación hacia la dirección del joystick
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // Aplicar la rotación directamente (sin suavización)
                transform.rotation = targetRotation;
            }
        }
    }

    // Método para rotar el personaje con el ratón
    public void RotateCharacterWithMouse(Vector3 mouseScreenPosition)
    {
        if(canRotate){
            // Convertimos la posición del mouse a una posición en el mundo del juego usando Raycast
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Obtenemos la posición en el mundo donde el raycast impacta
                Vector3 targetPosition = hit.point;

                // Calculamos la dirección desde el personaje hacia la posición del ratón
                Vector3 directionToLook = targetPosition - transform.position;
                directionToLook.y = 0; // Evitamos rotar en el eje Y (vertical)

                // Rotamos el personaje hacia la posición del mouse sin suavizar
                transform.rotation = Quaternion.LookRotation(-directionToLook);
            }
        }
    }

    public void SetRotationState(bool state){
        canRotate = state;
    }
}
