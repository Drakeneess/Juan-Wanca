using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class DoorItem : Item
{
    public float scaleReduction = 0.5f; // Porcentaje de reducción del tamaño del personaje
    public float cameraZoomAmount = 5f; // Cantidad que se acercará la cámara
    public Camera playerCamera; // Referencia a la cámara del jugador

    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de tener una referencia a la cámara
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Puedes cambiar esto si usas otra cámara
        }
    }

    public override void Action(Transform player)
    {
        // Reducir el tamaño del personaje
        player.localScale *= scaleReduction; // Ajustar el tamaño

        // Acercar la cámara
        if (playerCamera != null)
        {
            // Puedes ajustar la posición de la cámara
            Vector3 newPosition = playerCamera.transform.position;
            newPosition.z += cameraZoomAmount; // Acercar en el eje Z (ajusta esto según tu configuración)
            playerCamera.transform.position = newPosition;

            // Alternativamente, si usas una cámara con FOV:
            // playerCamera.fieldOfView -= cameraZoomAmount; // Esto requiere que 'cameraZoomAmount' sea apropiado
        }

        // Abrir la puerta
        GetComponent<Door>().OpenDoor();
    }
}
