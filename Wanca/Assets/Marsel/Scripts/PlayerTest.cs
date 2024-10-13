using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f; // Velocidad de movimiento del personaje
    public float rotationSpeed = 720f; // Velocidad de rotación del personaje
    public Rigidbody playerRigidbody; // Referencia al Rigidbody del jugador

    private Vector3 moveDirection; // Dirección del movimiento

    void Start()
    {
        // Asegurarse de que el Rigidbody está asignado
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Llamar al método que controla el movimiento del personaje
        HandleMovement();
    }

    // Método para manejar el movimiento del jugador
    private void HandleMovement()
    {
        // Obtener el input horizontal (A/D o flechas izquierda/derecha)
        float horizontal = Input.GetAxis("Horizontal");

        // Obtener el input vertical (W/S o flechas arriba/abajo)
        float vertical = Input.GetAxis("Vertical");

        // Crear un vector de dirección en función de los inputs
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Verificar si el personaje se está moviendo
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calcular el ángulo de rotación en función de la dirección de movimiento
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // Rotar suavemente al personaje hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Aplicar movimiento hacia adelante en la dirección de la rotación
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + move);
        }
    }
}
