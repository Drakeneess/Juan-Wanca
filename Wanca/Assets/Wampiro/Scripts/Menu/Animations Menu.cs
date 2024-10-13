using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsMenu : MonoBehaviour
{
    // Referencias a las partes del cuerpo
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;

    // Variables para controlar la animación
    private float animationSpeed = 20.0f;    // Velocidad de la animación de caminar
    private float idleSpeed = 2.0f;          // Velocidad de la animación de idle
    private float armRotationAngle = 45.0f;  // Grados que los brazos rotan al caminar
    private float legRotationAngle = 30.0f;  // Grados que las piernas rotan al caminar
    private float idleAmplitude = 5.0f;      // Amplitud de oscilación en idle
    private Vector2 directionMove = Vector2.zero;
    private bool animate = true;

    private Rigidbody rb;  // Referencia al Rigidbody del personaje

    // Enumeración para los estados de animación
    public enum AnimationState
    {
        Idle,
        Happy,
        Thoughtful,
        Guard,
        Wave
    }

    private AnimationState currentAnimationState = AnimationState.Idle; // Estado actual de la animación

    // Variables para almacenar las rotaciones actuales y objetivo
    private Quaternion targetLeftArmRotation;
    private Quaternion targetRightArmRotation;
    private Quaternion targetLeftLegRotation;
    private Quaternion targetRightLegRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en el objeto.");
        }

        // Iniciar la coroutine para las animaciones aleatorias
        StartCoroutine(RandomStaticAnimation());

        // Inicializar las rotaciones objetivo
        targetLeftArmRotation = leftArm.localRotation;
        targetRightArmRotation = rightArm.localRotation;
        targetLeftLegRotation = leftLeg.localRotation;
        targetRightLegRotation = rightLeg.localRotation;
    }

    void Update()
    {
        if (animate)
        {
            AnimateCharacter();
        }
        else
        {
            PlayStaticAnimation();
        }

        // Suavizar las rotaciones de las partes del cuerpo
        leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, targetLeftArmRotation, Time.deltaTime * 5);
        rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, targetRightArmRotation, Time.deltaTime * 5);
        leftLeg.localRotation = Quaternion.Lerp(leftLeg.localRotation, targetLeftLegRotation, Time.deltaTime * 5);
        rightLeg.localRotation = Quaternion.Lerp(rightLeg.localRotation, targetRightLegRotation, Time.deltaTime * 5);
    }

    void AnimateCharacter()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            Walk();
        }
        else
        {
            Idle();
        }
    }

    public void Walk()
    {
        float armRotation = Mathf.Sin(Time.time * animationSpeed) * armRotationAngle;
        targetLeftArmRotation = Quaternion.Euler(armRotation, 0, 0);
        targetRightArmRotation = Quaternion.Euler(-90, 0, 0);  // Invertido para balancear

        float legRotation = Mathf.Sin(Time.time * animationSpeed) * legRotationAngle;
        targetLeftLegRotation = Quaternion.Euler(legRotation * directionMove.x, 0, legRotation * directionMove.y);
        targetRightLegRotation = Quaternion.Euler(-legRotation * directionMove.x, 0, -legRotation * directionMove.y);
    }

    public void Idle()
    {
        float idleArmRotation = Mathf.Sin(Time.time * idleSpeed) * idleAmplitude;
        float idleLegRotation = Mathf.Sin(Time.time * idleSpeed) * (idleAmplitude / 2);

        targetLeftArmRotation = Quaternion.Euler(idleArmRotation, 0, 0);
        targetRightArmRotation = Quaternion.Euler(-90, 0, 0);
        
        targetLeftLegRotation = Quaternion.Euler(idleLegRotation, 0, 0);
        targetRightLegRotation = Quaternion.Euler(-idleLegRotation, 0, 0);
    }

    public void SetDirection(Vector2 newDirection)
    {
        directionMove.x = newDirection.y != 0 ? 1 : 0;
        directionMove.y = newDirection.x != 0 ? 1 : 0;
    }

    public void SetAnimate(bool newAnimate)
    {
        animate = newAnimate;
    }

    // Método para establecer el estado de animación estática
    public void SetStaticAnimation(AnimationState state)
    {
        currentAnimationState = state;
        animate = false; // Desactiva el movimiento
        PlayStaticAnimation();
    }

    private void PlayStaticAnimation()
    {
        switch (currentAnimationState)
        {
            case AnimationState.Happy:
                HappyAnimation();
                break;
            case AnimationState.Thoughtful:
                ThoughtfulAnimation();
                break;
            case AnimationState.Guard:
                GuardAnimation();
                break;
            case AnimationState.Wave:
                WaveAnimation();
                break;
            default:
                Idle(); // Vuelve al estado de idle por defecto
                break;
        }
    }

    // Implementación de las animaciones estáticas
    private void HappyAnimation()
    {
        // Ejemplo de animación feliz
        targetLeftArmRotation = Quaternion.Euler(30, 0, 0); // Levanta el brazo
        targetRightArmRotation = Quaternion.Euler(30, 0, 0); // Levanta el brazo
        targetLeftLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
        targetRightLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
    }

    private void ThoughtfulAnimation()
    {
        // Ejemplo de animación pensativa
        targetLeftArmRotation = Quaternion.Euler(0, 0, 0); // Brazo normal
        targetRightArmRotation = Quaternion.Euler(-20, 0, 0); // Brazo en la barbilla
        targetLeftLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
        targetRightLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
    }

    private void GuardAnimation()
    {
        // Ejemplo de animación en guardia
        targetLeftArmRotation = Quaternion.Euler(0, 0, 0); // Brazo normal
        targetRightArmRotation = Quaternion.Euler(90, 0, 0); // Brazo arriba
        targetLeftLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
        targetRightLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
    }

    private void WaveAnimation()
    {
        // Ejemplo de animación de saludo
        targetLeftArmRotation = Quaternion.Euler(45, 0, 0); // Brazo levantado
        targetRightArmRotation = Quaternion.Euler(0, 0, 0); // Brazo normal
        targetLeftLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
        targetRightLegRotation = Quaternion.Euler(0, 0, 0); // Pierna normal
    }

    // Coroutine para seleccionar animaciones estáticas aleatorias
    private IEnumerator RandomStaticAnimation()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio entre 2 y 5 segundos
            float waitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitTime);

            // Seleccionar aleatoriamente un estado de animación
            AnimationState randomState = (AnimationState)Random.Range(1, System.Enum.GetValues(typeof(AnimationState)).Length);
            SetStaticAnimation(randomState);
        }
    }
}
