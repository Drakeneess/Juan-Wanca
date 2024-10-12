using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 initialOffset;  // Offset inicial entre la cámara y el jugador
    public float smoothSpeed = 0.125f;  // Velocidad para suavizar el seguimiento
    public float shakeDuration = 0.5f;  // Duración del temblor

    private Transform player;  // Referencia al transform del jugador
    private Coroutine shakeCoroutine;  // Almacena la coroutine del temblor

    // Start is called before the first frame update
    void Start()
    {
        // Encuentra al jugador si no está asignado manualmente
        if (player == null)
        {
            player = FindObjectOfType<PlayerGeneralController>().transform;
        }

        // Calcula el offset inicial entre la cámara y el jugador
        initialOffset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Mantén la distancia inicial (offset) entre la cámara y el jugador
        Vector3 desiredPosition = player.position + initialOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Opcional: Haz que la cámara apunte hacia el jugador
        // transform.LookAt(player);
    }

    // Método para iniciar el temblor de la cámara
    public void ShakeCamera(float shakeMagnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine); // Detener el temblor anterior si ya está en curso
        }
        shakeCoroutine = StartCoroutine(Shake(shakeMagnitude)); // Iniciar una nueva coroutine para el temblor
    }

    // Coroutine para manejar el temblor
    private IEnumerator Shake(float shakeMagnitude)
    {
        Vector3 originalPosition = transform.position; // Guarda la posición original de la cámara
        float elapsed = 0f; // Tiempo transcurrido

        while (elapsed < shakeDuration)
        {
            // Calcula un nuevo offset aleatorio para el temblor
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.position = originalPosition + new Vector3(x, y, 0); // Aplica el temblor

            elapsed += Time.deltaTime; // Aumenta el tiempo transcurrido
            yield return null; // Espera el siguiente frame
        }

        transform.position = originalPosition; // Restaura la posición original al finalizar el temblor
    }
}
