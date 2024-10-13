using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    public Transform character;  // Referencia al transform del personaje
    public float distanceFromCharacter = 5f;  // Distancia de la cámara al personaje
    public float rotationSpeed = 5f;  // Velocidad de rotación de la cámara
    public Vector2 rotationLimits = new Vector2(-20, 60);  // Límites de rotación vertical (para no mirar el suelo o el cielo)

    private float currentYaw = 0f;  // Rotación horizontal
    private float currentPitch = 0f;  // Rotación vertical
    private Menu menu;  // Referencia al script del menú para obtener el movimiento de cámara

    private void Start()
    {
        // Encontrar el script Menu en la escena
        menu = FindObjectOfType<Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener el movimiento de la cámara desde el script Menu
        Vector2 direction = menu.GetCameraMovement();

        // Actualizar el ángulo horizontal y vertical de acuerdo con la entrada del jugador
        currentYaw += direction.x * rotationSpeed;
        currentPitch -= direction.y * rotationSpeed;  // Restar para que moverse hacia arriba en el mouse baje la cámara

        // Limitar la rotación vertical
        currentPitch = Mathf.Clamp(currentPitch, rotationLimits.x, rotationLimits.y);

        // Aplicar la rotación de la cámara
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        transform.position = character.position - rotation * Vector3.forward * distanceFromCharacter;

        // Hacer que la cámara siempre mire al personaje
        transform.LookAt(character);
    }
}
