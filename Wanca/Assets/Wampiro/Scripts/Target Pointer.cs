using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    private bool isTargetOnView = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Si no hay objetivo visible, ajusta la rotación del puntero para que mire hacia arriba
        if (!isTargetOnView) 
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Método para ajustar la posición del puntero en el suelo cuando no hay objetivo
    public void PointerPosition(Vector3 newPosition)
    {
        // Ajustar la posición en el suelo (y=0)
        transform.position = new Vector3(newPosition.x, 0, newPosition.z);
        isTargetOnView = false;
    }

    // Método para mover el puntero cuando detecta un objetivo
    // Método para mover el puntero cuando detecta un objetivo
    public void SetTarget(Vector3 targetPosition, Transform targetObjective)
    {
        isTargetOnView = true;
        transform.position = targetPosition;

        // Hacer que el puntero mire al origen (personaje)
        transform.LookAt(targetObjective);

        // Aplicar una rotación de -90 grados en el eje Y para ajustar el ángulo como deseas
        transform.Rotate(-90f, 0f, 0f);
    }
    public void TargetControl(bool state){
        gameObject.SetActive(state);
    }
}
