using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IluminationController : MonoBehaviour
{
    public Material celShadingMaterial;  // Referencia al material de cel-shading
    public List<ArtificialLight> artificialLights; // Lista de todas las luces artificiales en la escena

    // Nombres de los parámetros en el shader
    protected readonly string lightDirectionProperty = "_Target_Position"; // Nombre del parámetro de dirección de luz en el shader
    protected readonly string intensityProperty = "_Intensity";  // Nombre del parámetro de intensidad en el shader

    private ArtificialLight closestLight;  // Luz más cercana

    // Start is called before the first frame update
    void Start()
    {
        celShadingMaterial = GetComponent<MeshRenderer>().material;

        if (celShadingMaterial == null)
        {
            Debug.LogError("No se ha asignado el material de cel-shading.");
        }

        // Buscar todas las luces ArtificialLight en la escena
        artificialLights = new List<ArtificialLight>(FindObjectsOfType<ArtificialLight>());

        if (artificialLights.Count == 0)
        {
            Debug.LogError("No se han encontrado luces artificiales en la escena.");
        }

        // Agregar IluminationController a todos los hijos
        AddIluminationControllerToChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (celShadingMaterial != null && artificialLights.Count > 0)
        {
            // Encuentra la luz más cercana y actualiza la iluminación
            FindClosestLight();
            UpdateLighting();
        }
    }

    // Método para encontrar la luz artificial más cercana
    private void FindClosestLight()
    {
        float closestDistance = Mathf.Infinity; // Iniciar con una distancia grande

        foreach (ArtificialLight light in artificialLights)
        {
            float distance = Vector3.Distance(transform.position, light.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestLight = light;  // Actualizar la luz más cercana
            }
        }
    }

    // Método para actualizar la dirección y la intensidad de la luz en el shader
    private void UpdateLighting()
    {
        if (closestLight != null)
        {
            // Obtiene la dirección de la luz más cercana
            Vector3 lightDirection = closestLight.transform.position;

            // Configura la dirección de la luz en el material
            celShadingMaterial.SetVector(lightDirectionProperty, lightDirection);
            
            // Establecer intensidad entre 0.2 y 0.8 según la distancia
            float distance = Vector3.Distance(transform.position, closestLight.transform.position);
            float intensity = Mathf.Clamp(0.8f / distance, 0.2f, 0.8f);


            // Configura la intensidad en el material
            celShadingMaterial.SetFloat(intensityProperty, intensity);
        }
    }

    // Método para agregar el componente IluminationController a todos los hijos del objeto
    private void AddIluminationControllerToChildren()
    {
        // Obtener todos los hijos del objeto principal (incluyéndose a sí mismo si quieres)
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            // Si el hijo tiene un MeshRenderer pero no tiene IluminationController, se lo añadimos
            if (child.gameObject != this.gameObject && child.GetComponent<IluminationController>() == null && child.GetComponent<MeshRenderer>() != null)
            {
                IluminationController iluminationController = child.gameObject.AddComponent<IluminationController>();

                // Asignar las mismas luces y material a los hijos
                iluminationController.artificialLights = this.artificialLights;
                iluminationController.celShadingMaterial = this.celShadingMaterial;
            }
        }
    }

    // Método para establecer nuevas luces artificiales (en caso de que cambien dinámicamente)
    public void SetLights(List<ArtificialLight> newArtificialLights)
    {
        artificialLights = newArtificialLights;
    }
}
