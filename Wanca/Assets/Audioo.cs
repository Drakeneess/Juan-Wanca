using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioo : MonoBehaviour
{
    AudioSource audioSource;
    private void Start() {
        
    audioSource =GetComponent<AudioSource>();
if (audioSource != null)
{

    // Reproducir el sonido automáticamente
    audioSource.Play();
}
    }
}
