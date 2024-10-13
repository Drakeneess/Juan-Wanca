using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para manejar la UI

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        // Para la pantalla de carga
        public GameObject loadingScreen; // Referencia a la pantalla de carga en la UI

        // Use this for initialization
        void Start()
        {
            asource = GetComponent<AudioSource>();

            // Asegurarse de que la pantalla de carga esté desactivada al inicio
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Detectar la tecla 'E' para abrir/cerrar la puerta
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenDoor();
            }

            // Manejar la rotación de la puerta dependiendo si está abierta o cerrada
            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
            }
        }

        // Método para abrir/cerrar la puerta y mostrar la pantalla de carga
        public void OpenDoor()
        {
            open = !open;

            // Reproducir sonido
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();

            // Si se abre la puerta, mostrar la pantalla de carga
            if (open && loadingScreen != null)
            {
                StartCoroutine(ShowLoadingScreen());
                DungeonCreator dungeonCreator = new DungeonCreator();
                dungeonCreator.SelectRandomTheme();
                dungeonCreator.CreateDungeon();
                dungeonCreator.TeleportToSurface();
            }
                   
                
        }

        // Corrutina para mostrar la pantalla de carga durante un tiempo determinado
        IEnumerator ShowLoadingScreen()
        {
            // Activar la pantalla de carga
            loadingScreen.SetActive(true);

            // Simular un tiempo de carga (ejemplo: 2 segundos)
            yield return new WaitForSeconds(2);

            // Desactivar la pantalla de carga después de la espera
            loadingScreen.SetActive(false);
        }
    }
}
