using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterAnimations : MonoBehaviour
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
    private Vector2 directionMove=Vector2.zero;
    private bool animate = true;

    private Rigidbody rb;           // Referencia al Rigidbody del cubo
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en el objeto.");
        }
    }

    void Update()
    {
        if (animate)
        {
            AnimateCharacter();
        }
    }

    void AnimateCharacter()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            Walk();
        }
        else
        {
            Iddle();
        }
    }

    
    public void Walk(){
        float armRotation = Mathf.Sin(Time.time * animationSpeed) * armRotationAngle;
        leftArm.localRotation = Quaternion.Euler(armRotation, 0, 0);
        rightArm.localRotation = Quaternion.Euler(-90, 0, 0);  // Invertido para balancear

        float legRotation = Mathf.Sin(Time.time * animationSpeed) * legRotationAngle;
        leftLeg.localRotation = Quaternion.Euler(legRotation*directionMove.x, 0, legRotation*directionMove.y);
        rightLeg.localRotation = Quaternion.Euler(-legRotation*directionMove.x, 0, -legRotation*directionMove.y);
    }
    public void Iddle(){
        float idleArmRotation = Mathf.Sin(Time.time * idleSpeed) * idleAmplitude;
        float idleLegRotation = Mathf.Sin(Time.time * idleSpeed) * (idleAmplitude / 2);

        leftArm.localRotation = Quaternion.Euler(idleArmRotation, 0, 0);
        rightArm.localRotation = Quaternion.Euler(-90, 0, 0);

        leftLeg.localRotation = Quaternion.Euler(idleLegRotation, 0, 0);
        rightLeg.localRotation = Quaternion.Euler(-idleLegRotation, 0, 0);
    }
    public void SetDirection(Vector2 newDirection){
        directionMove.x=newDirection.y!=0? 1 : 0;
        directionMove.y=newDirection.x!=0? 1 : 0;
    }

    public void SetAnimate(bool newAnimate){
        animate = newAnimate;
    }
}
