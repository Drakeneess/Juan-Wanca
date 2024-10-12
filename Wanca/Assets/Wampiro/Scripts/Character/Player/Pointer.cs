using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float distance = 10f;               // Distancia máxima del rayo.
    public LayerMask ContactLayers;            // Capas para colisiones.
    public LineRenderer line;                  // Componente LineRenderer para la línea.
    public float offset = 0.5f;                // Offset vertical del rayo.
    public TargetPointer target;               // Referencia al TargetPointer.
    private Ray ray;                           // Rayo para las detecciones.
    private RaycastHit hit;                    // Resultado del rayo.
    private Vector3 finalPosition;
    private bool canAim = true;                // Control para la activación del puntero.

    // Start is called before the first frame update
    void Start()
    {
        finalPosition = transform.position - (transform.up * (distance + offset));
        line.SetPosition(0, transform.position - (transform.up * offset));
        line.SetPosition(1, finalPosition);
        target.PointerPosition(finalPosition);
    }

    // Método para activar o desactivar el apuntado.
    public void SetPointerActive(bool state)
    {
        canAim = state;
        line.enabled = state; // Activar o desactivar la visualización de la línea.
        target.TargetControl(state); // Activar o desactivar el control del TargetPointer.
    }

    // FixedUpdate para controlar el puntero solo cuando está activado.
    private void FixedUpdate()
    {
        if (canAim)
        {
            Vector3 startPoint = transform.position - (transform.up * offset);
            line.SetPosition(0, startPoint);

            // Crear el rayo desde la posición desplazada hacia abajo.
            ray = new Ray(startPoint, -transform.up);

            // Verificar si el rayo golpea algo.
            if (Physics.Raycast(ray, out hit, distance, ContactLayers))
            {
                line.SetPosition(1, hit.point);
                if (hit.transform.gameObject.layer == 6)
                {
                    target.PointerPosition(hit.point);
                }
                else
                {
                    target.SetTarget(hit.point, hit.transform);
                }
            }
            else
            {
                line.SetPosition(1, startPoint + (-transform.up * distance));
                target.PointerPosition(transform.position - (transform.up * (distance + offset)));
            }
        }
    }
}
