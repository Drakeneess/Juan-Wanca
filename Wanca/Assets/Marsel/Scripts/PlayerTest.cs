using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f; // Velocidad de movimiento del personaje
    public float rotationSpeed = 720f; // Velocidad de rotaci�n del personaje
    public Rigidbody playerRigidbody; // Referencia al Rigidbody del jugador

    private Vector3 moveDirection; // Direcci�n del movimiento

    void Start()
    {
        // Asegurarse de que el Rigidbody est� asignado
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Llamar al m�todo que controla el movimiento del personaje
        HandleMovement();
    }

    // M�todo para manejar el movimiento del jugador
    private void HandleMovement()
    {
        // Obtener el input horizontal (A/D o flechas izquierda/derecha)
        float horizontal = Input.GetAxis("Horizontal");

        // Obtener el input vertical (W/S o flechas arriba/abajo)
        float vertical = Input.GetAxis("Vertical");

        // Crear un vector de direcci�n en funci�n de los inputs
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Verificar si el personaje se est� moviendo
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calcular el �ngulo de rotaci�n en funci�n de la direcci�n de movimiento
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // Rotar suavemente al personaje hacia la direcci�n de movimiento
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Aplicar movimiento hacia adelante en la direcci�n de la rotaci�n
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + move);
        }
    }
}
