using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : Menu
{
    public TextMeshProUGUI uGUI;
    public Button restartButton;
    public Button quitButton;

    private string[] funnyMessages = {
        "¡Oh no! ¡Te han atrapado en un juego de 'Vivo o Muerto'!",
        "¡No te preocupes! La próxima vez, intenta no caer en el abismo.",
        "Parece que has encontrado el botón de 'muerte instantánea'. Muy valiente.",
        "¿Es un pájaro? ¿Es un avión? ¡No! ¡Es tu vida que se desmorona!",
        "Recuerda: siempre puedes culpar al control remoto.",
        "¿Te sientes muerto por dentro? ¡Ahora estás muerto por fuera!",
        "La muerte es solo otra forma de decir 'necesito un descanso'.",
        "¡Ups! Alguien se olvidó de usar el botón de 'salto'.",
        "A veces, la vida es como un videojuego... ¡Y a veces mueres en la primera pantalla!",
        "¿Estás seguro de que ese era un enemigo y no solo un amigo disfrazado?",
        "¡No te preocupes! ¡Eres un clásico, como los errores de Windows!",
        "Ah, la muerte... la forma más rápida de reiniciar la diversión.",
        "¿Snake? ¿Snake? ¡SNAAAAAKE!",
        "¿Recuerdas que dijimos que este juego sería 'fácil'? ¡Sorpresa!",
        "¡No te preocupes! Solo estás en el nivel de prueba de la vida.",
        "Has muerto... Pero no te preocupes, ¡respawneas en 3, 2, 1!",
        "Parece que el 'Game Over' se ha llevado tu puntuación.",
        "Eres como Mario en el mundo 1-2... ¡Demasiado pronto para morir!",
        "Te has dado cuenta de que este no es Dark Souls, ¿verdad?",
        "¿Por qué cruzó el héroe la carretera? Para evitar la muerte, pero claramente no lo logró.",
        "¿Eres un juego de cartas? Porque ¡estás fuera de la mano!",
        "¡Vaya! Eso fue tan efectivo como un hechizo de 'Desvanecimiento'.",
        "¿Eres un glitch? Porque estás en la parte equivocada del juego.",
        "Has muerto como un verdadero guerrero... o un NPC despistado.",
        "No te preocupes, esta es solo una 'misión secundaria'.",
        "¿Quién necesita vidas extra? ¡Es más divertido morir!",
        "No llames a la asistencia técnica; esta vez no hay vuelta atrás.",
        "¡Oh, genial! Justo lo que necesitábamos: un 'Game Over' gratuito."
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Asignar las funciones a los botones
        restartButton.onClick.AddListener(ReloadScene);
        quitButton.onClick.AddListener(LoadMainMenu);

        // Establecer un mensaje gracioso
        uGUI.text = GetRandomFunnyMessage();
    }
    // Función para obtener un mensaje gracioso aleatorio
    string GetRandomFunnyMessage()
    {
        int randomIndex = Random.Range(0, funnyMessages.Length);
        return funnyMessages[randomIndex];
    }

    // Función para recargar la escena actual (Reintentar)
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Recargar la escena actual
    }

    // Función para cargar la escena principal (Escena 0) cuando se selecciona Quit
    void LoadMainMenu()
    {
        SceneManager.LoadScene(0);  // Cargar la escena con el índice 0 (menú principal)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
