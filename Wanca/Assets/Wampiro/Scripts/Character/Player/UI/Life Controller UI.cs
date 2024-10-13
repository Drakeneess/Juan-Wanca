using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeControllerUI : MonoBehaviour
{
    public GameObject lifeImagePrefab; // Prefab de la imagen de vida a instanciar.
    public Color[] states; // Diferentes colores para los estados de vida (por ejemplo: lleno, medio, vacío).

    private PlayerGeneralController character;
    private int maxLife;
    private int currentLife;
    private List<Image> lifeImages = new List<Image>(); // Lista para almacenar las imágenes de vida.

    // Start is called before the first frame update
    void Start()
    {
        character = FindObjectOfType<PlayerGeneralController>();
        maxLife = Mathf.FloorToInt(character.maxHealth);
        currentLife = Mathf.FloorToInt(character.GetHealth());

        // Crear las imágenes según la vida máxima
        for (int i = 0; i < maxLife; i++)
        {
            GameObject lifeImageObj = Instantiate(lifeImagePrefab, transform);
            lifeImageObj.transform.SetParent(transform);
            Image lifeImage = lifeImageObj.GetComponent<Image>();
            lifeImages.Add(lifeImage); // Añadir la imagen a la lista.
        }

        // Actualizar las imágenes de vida según la vida actual.
        UpdateLifeImages();
    }

    // Update is called once per frame
    void Update()
    {
        currentLife = Mathf.FloorToInt(character.GetHealth());
        UpdateLifeImages();
        if(currentLife<=0){
            Destroy(gameObject);
        }
    }

    // Método para actualizar las imágenes de vida.
    private void UpdateLifeImages()
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (i < currentLife)
            {
                // Si el índice es menor que la vida actual, el estado es "lleno" (color del índice 0).
                lifeImages[i].color = states[0];
            }
            else if (i < maxLife)
            {
                // Si está en medio, puedes usar otro estado (color del índice 1 o vacío).
                lifeImages[i].color = states[1]; // Cambia esto según tu diseño.
            }
            else
            {
                // Si el índice es mayor que la vida actual, el estado es "vacío" (color del índice 2).
                lifeImages[i].color = states[2];
            }
        }
    }
}
