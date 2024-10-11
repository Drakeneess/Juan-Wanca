using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCharacter : MonoBehaviour
{
    // Variables para el movimiento del personaje
    public float moveSpeed = 5.0f;  // Velocidad de movimiento
    private Rigidbody rb;           // Referencia al Rigidbody del cubo
    private CharacterAnimations characterAnimations;
    private  bool canMove = true;


    void Start()
    {
        // Obtener el Rigidbody del cubo
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en el objeto.");
        }
        characterAnimations=GetComponent<CharacterAnimations>();
    }

    public void MoveCharacter(Vector2 direction)
    {
        if(canMove){
            // Crear el vector de movimiento en espacio local (relacionado con la rotación del personaje)
            Vector3 moveDirection = new Vector3(direction.x,0,direction.y);//transform.right * direction.x + transform.forward * direction.y;

            // Aplicar movimiento al Rigidbody si hay alguna dirección
            if (moveDirection.magnitude >= 0.1f)
            {
                // Mover el personaje sin rotar
                Vector3 movement = moveDirection * moveSpeed;
                rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);  // Conservar la velocidad vertical (gravedad)
                characterAnimations.SetDirection(direction);
            }
            else
            {
                // Si no se mueve, establecer la velocidad horizontal en 0
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
    }

    public void SetMoveState(bool state){
        canMove = state;
    }
}
