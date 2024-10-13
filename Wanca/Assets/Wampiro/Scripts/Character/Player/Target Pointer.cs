using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    public float normalRotationSpeed = 50f;  // Velocidad de rotación normal
    public float targetRotationSpeed = 100f; // Velocidad de rotación cuando apunta a un objetivo
    public float scale = 1.1f;
    public Transform trigs;

    private bool isTargetOnView = false;
    private bool targetInPosition = false;
    private Vector3 enlargedScale;  // Ajusta este valor para agrandar el puntero.
    private Vector3 normalScale;

    private void Awake() 
    {
        // Establecer la rotación inicial
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        enlargedScale = new Vector3(scale, scale, scale);
        normalScale = transform.localScale;  // Guardamos el tamaño normal al inicio.
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTargetOnView)
        {
            // Restablecer la rotación a 90 grados si no hay objetivo en vista
            if (!targetInPosition)
            {
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                targetInPosition = true;
            }

            // Rotación normal cuando no está apuntando a un objetivo
            transform.Rotate(Vector3.up, normalRotationSpeed * Time.deltaTime, Space.World);
            transform.localScale = normalScale;  // Restaurar el tamaño normal si no hay objetivo.
        }
        else
        {
            // Lógica adicional si hay un objetivo en vista (puedes agregar rotación adicional aquí)
            // Por ejemplo: transformar la rotación para mirar hacia el objetivo
        }
    }

    // Método para ajustar la posición del puntero en el suelo cuando no hay objetivo
    public void PointerPosition(Vector3 newPosition)
    {
        // Ajustar la posición en el suelo (y=0)
        transform.position = new Vector3(newPosition.x, 0, newPosition.z);
        isTargetOnView = false;
    }

    // Método para mover el puntero cuando detecta un objetivo
    public void SetTarget(Vector3 targetPosition, Transform targetObjective)
    {
        isTargetOnView = true;
        targetInPosition = false;
        transform.position = targetPosition;

        // Hacer que el puntero mire al objetivo
        transform.LookAt(targetObjective);

        // Aumentar el tamaño del puntero cuando apunta a un objetivo
        transform.localScale = enlargedScale;
    }

    // Método para controlar la visibilidad del puntero
    public void TargetControl(bool state)
    {
        gameObject.SetActive(state);
    }
}
