using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : MonoBehaviour
{
    public float tackleMaxForce = 20f;  // Fuerza máxima de la tacleada
    public float tackleJumpForce = 5f;  // Fuerza del salto en la tacleada
    public float tackleDuration = 0.5f; // Duración de la tacleada
    public float tackleLiftAngle = 30f; // Ángulo de elevación de los brazos durante la tacleada
    public float recoveryDuration = 0.3f; // Duración del tiempo de recuperación o "levantarse"
    public float postTackleRotationDuration = 0.3f; // Duración para rotar a 180 grados después de la tacleada

    private bool isTackling = false; // Estado de tacleada
    private Transform leftArm;
    private Transform rightArm;
    private Transform leftLeg;
    private Transform rightLeg;
    private Rigidbody rb;           // Referencia al Rigidbody del cubo
    private CharacterAnimations animations;
    private PlayerGeneralController generalController;
    public Fist fist;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animations = GetComponent<CharacterAnimations>();
        generalController = GetComponent<PlayerGeneralController>();
        fist.punchDamage=2;

        leftArm = animations.leftArm;
        rightArm = animations.rightArm;
        leftLeg = animations.leftLeg;
        rightLeg = animations.leftLeg;
    }

    public void TackleAttack()
    {
        if (!isTackling)
        {
            StartCoroutine(TackleRoutine());
        }
    }

    private IEnumerator TackleRoutine()
    {
        isTackling = true;
        generalController.SetStates(!isTackling);
        fist.EnableCollider(true);

        // *** Paso 1: Detener cualquier movimiento previo ***
        rb.velocity = Vector3.zero;  // Detener la velocidad en todas las direcciones
        rb.angularVelocity = Vector3.zero;  // Detener cualquier rotación previa

        // *** Paso 2: Levantar los brazos y saltar ***
        float startTime = Time.time;
        float endTime = startTime + tackleDuration;

        // Aplicar un impulso hacia arriba para el salto
        rb.AddForce(Vector3.up * tackleJumpForce, ForceMode.Impulse);

        // Guardar la rotación original del personaje
        Quaternion originalRotation = transform.rotation;

        // *** Paso 3: Aplicar la fuerza hacia adelante con aceleración durante la tacleada ***
        while (Time.time < endTime)
        {
            // Progresión de la tacleada
            float progress = (Time.time - startTime) / tackleDuration;
            
            // Aumentar la fuerza gradualmente para simular aceleración
            float currentForce = Mathf.Lerp(0, tackleMaxForce, progress);

            // Aplicar la fuerza hacia adelante con aceleración
            rb.AddForce(transform.forward * currentForce * Time.deltaTime, ForceMode.VelocityChange);

            // Levantar los brazos
            leftArm.localRotation = Quaternion.Euler(tackleLiftAngle * progress, 0, 0);
            rightArm.localRotation = Quaternion.Euler(tackleLiftAngle * progress, 0, 0);

            // Inclinar el personaje (opcional)
            transform.localRotation = Quaternion.Lerp(originalRotation, originalRotation * Quaternion.Euler(40, 0, 0), progress);

            yield return null; // Esperar hasta el siguiente frame
        }

        // *** Paso 4: Girar a 180° después de la tacleada ***
        yield return StartCoroutine(PostTackleRotation());
        fist.EnableCollider(false);

        // *** Paso 5: Animación de levantarse ***
        yield return StartCoroutine(RecoverFromTackle(originalRotation));

        // Finaliza la tacleada
        isTackling = false;
        generalController.SetStates(!isTackling);
    }

    private IEnumerator PostTackleRotation()
    {
        // Guardar la rotación original
        Quaternion startRotation = transform.localRotation;

        // Calcular la rotación objetivo a 180°
        Quaternion targetRotation = startRotation * Quaternion.Euler(180, 0, 0);

        // Rotar los brazos a -180°
        Quaternion targetLeftArmRotation = Quaternion.Euler(-180, 0, 0);
        Quaternion targetRightArmRotation = Quaternion.Euler(-180, 0, 0);

        // *** Realizar la rotación en el tiempo definido ***
        float postTackleStartTime = Time.time;
        float postTackleEndTime = postTackleStartTime + postTackleRotationDuration;

        while (Time.time < postTackleEndTime)
        {
            float progress = (Time.time - postTackleStartTime) / postTackleRotationDuration;

            // Rotar el personaje y los brazos gradualmente a la posición final
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, targetLeftArmRotation, progress);
            rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, targetRightArmRotation, progress);

            yield return null;  // Esperar el siguiente frame
        }

        // Asegurarse de que la rotación termine exactamente en la posición deseada
        transform.localRotation = targetRotation;
        leftArm.localRotation = targetLeftArmRotation;
        rightArm.localRotation = targetRightArmRotation;
    }

    private IEnumerator RecoverFromTackle(Quaternion originalRotation)
    {
        // *** Animación de levantarse ***
        float recoveryStartTime = Time.time;
        float recoveryEndTime = recoveryStartTime + recoveryDuration;

        while (Time.time < recoveryEndTime)
        {
            float progress = (Time.time - recoveryStartTime) / recoveryDuration;

            // Gradualmente retornar a la posición original (levantar al personaje)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, progress);

            // También puedes incluir aquí ajustes para brazos y piernas
            leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, Quaternion.Euler(0, 0, 0), progress);
            rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, Quaternion.Euler(0, 0, 0), progress);

            yield return null;  // Esperar el siguiente frame
        }

        // Asegurarse de que la rotación vuelva exactamente a su valor original
        transform.localRotation = originalRotation;
        leftArm.localRotation = Quaternion.Euler(0, 0, 0);
        rightArm.localRotation = Quaternion.Euler(-90, 0, 0);
    }
}
