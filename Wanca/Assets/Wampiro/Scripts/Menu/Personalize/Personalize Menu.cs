using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalizeMenu : MonoBehaviour
{
    public Material bodyMaterial;  // Material del cuerpo
    public Material limbMaterial;   // Material de las extremidades
    public Transform BodyTest;      // GameObject del cuerpo de prueba
    public Transform[] LimbTest;    // Array de GameObjects de las extremidades de prueba

    public Button color1;  // Botón para el color del cuerpo
    public Button color2;  // Botón para la sombra del cuerpo
    public Button color3;  // Botón para el color de las extremidades
    public Button color4;  // Botón para la sombra de las extremidades

    public Slider sliderR; // Slider para el rojo
    public Slider sliderG; // Slider para el verde
    public Slider sliderB; // Slider para el azul

    public TMP_InputField inputR; // Input para el rojo
    public TMP_InputField inputG; // Input para el verde
    public TMP_InputField inputB; // Input para el azul

    public Image result; // Imagen para mostrar el color resultante
    private readonly string colorProperty = "_Color";           // Propiedad para el color
    private readonly string shadowColorProperty = "_Shadow_Color"; // Propiedad para la sombra

    // Start is called before the first frame update
    void Start()
    {
        // Configurar el rango de los sliders
        sliderR.maxValue = 255;
        sliderG.maxValue = 255;
        sliderB.maxValue = 255;

        // Asignar eventos a los sliders
        sliderR.onValueChanged.AddListener((value) => UpdateResultColor());
        sliderG.onValueChanged.AddListener((value) => UpdateResultColor());
        sliderB.onValueChanged.AddListener((value) => UpdateResultColor());

        // Asignar eventos a los input fields
        inputR.onEndEdit.AddListener((value) => UpdateResultColorFromInput());
        inputG.onEndEdit.AddListener((value) => UpdateResultColorFromInput());
        inputB.onEndEdit.AddListener((value) => UpdateResultColorFromInput());

        // Asignar eventos a los botones
        color1.onClick.AddListener(SetBodyColor);
        color2.onClick.AddListener(SetBodyShadowColor);
        color3.onClick.AddListener(SetLimbColor);
        color4.onClick.AddListener(SetLimbShadowColor);

        // Inicializar el color de la imagen resultante
        UpdateResultColor();
    }

    // Actualiza el color del resultado para la vista previa
    void UpdateResultColor()
    {
        // Obtener los valores de los sliders y convertirlos a un rango de 0 a 1
        Color color = new Color(sliderR.value / 255f, sliderG.value / 255f, sliderB.value / 255f);
        result.color = color; // Establecer el color en la imagen de resultado

        // Actualizar los campos de entrada para reflejar el valor de los sliders
        inputR.text = sliderR.value.ToString();
        inputG.text = sliderG.value.ToString();
        inputB.text = sliderB.value.ToString();
    }

    // Actualiza el color basado en los input fields
    void UpdateResultColorFromInput()
    {
        if (int.TryParse(inputR.text, out int r) && int.TryParse(inputG.text, out int g) && int.TryParse(inputB.text, out int b))
        {
            // Asegurarse de que los valores están en el rango de 0 a 255
            r = Mathf.Clamp(r, 0, 255);
            g = Mathf.Clamp(g, 0, 255);
            b = Mathf.Clamp(b, 0, 255);

            // Crear un nuevo color a partir de los valores de los inputs
            Color color = new Color(r / 255f, g / 255f, b / 255f);
            result.color = color; // Establecer el color en la imagen de resultado

            // Actualizar los sliders para reflejar los valores de los inputs
            sliderR.value = r;
            sliderG.value = g;
            sliderB.value = b;
        }
    }

    // Asigna el color actual al material del cuerpo y a los GameObjects de prueba
    void SetBodyColor()
    {
        bodyMaterial.SetColor(colorProperty, result.color);
        if (BodyTest != null)
        {
            // Aplicar el color al GameObject de prueba
            BodyTest.GetComponent<Renderer>().sharedMaterial = bodyMaterial; // Usar el material compartido
        }
    }

    // Asigna el color actual como sombra del material del cuerpo
    void SetBodyShadowColor()
    {
        bodyMaterial.SetColor(shadowColorProperty, result.color);
        if (BodyTest != null)
        {
            // Aplicar la sombra al GameObject de prueba
            BodyTest.GetComponent<Renderer>().sharedMaterial = bodyMaterial; // Usar el material compartido
        }
    }

    // Asigna el color actual al material de las extremidades y a los GameObjects de prueba
    void SetLimbColor()
    {
        limbMaterial.SetColor(colorProperty, result.color);
        foreach (Transform limb in LimbTest)
        {
            if (limb != null)
            {
                // Aplicar el color a cada GameObject de prueba
                limb.GetComponent<Renderer>().sharedMaterial = limbMaterial; // Usar el material compartido
            }
        }
    }

    // Asigna el color actual como sombra del material de las extremidades
    void SetLimbShadowColor()
    {
        limbMaterial.SetColor(shadowColorProperty, result.color);
        foreach (Transform limb in LimbTest)
        {
            if (limb != null)
            {
                // Aplicar la sombra a cada GameObject de prueba
                limb.GetComponent<Renderer>().sharedMaterial = limbMaterial; // Usar el material compartido
            }
        }
    }
}
