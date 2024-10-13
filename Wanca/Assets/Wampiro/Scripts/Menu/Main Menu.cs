using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    public Button startButton;
    public Button personalizeButton;
    public Button quitButton;
    public GameObject subMenu;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Asignar las funciones a los botones
        startButton.onClick.AddListener(StartGame);
        personalizeButton.onClick.AddListener(OpenPersonalizeMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Función para empezar el juego
    public void StartGame()
    {
        Debug.Log("Iniciando el juego...");
        // Aquí agregarías la lógica para cambiar de escena, por ejemplo:
        SceneManager.LoadScene(1);
    }

    // Función para abrir el menú de personalización
    public void OpenPersonalizeMenu()
    {
        Debug.Log("Abriendo el menú de personalización...");
        // Lógica para abrir el menú de personalización
    }

    // Función para abrir el menú de opciones
    public void OpenOptionsMenu()
    {
        Debug.Log("Abriendo el menú de opciones...");
        // Lógica para abrir el menú de opciones
        subMenu.SetActive(true);
    }

    // Función para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
