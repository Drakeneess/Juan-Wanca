using System.Collections;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public Transform rightArm;  // Referencia al brazo derecho para el puñetazo
    public Fist fist;  // Referencia al componente Fist
    private CharacterGeneralController characterGeneralController;

    private bool isPunching = false;  // Estado para saber si está lanzando un puñetazo
    private float punchDuration = 0.2f; // Duración de la animación del puñetazo (más rápida)
    private float recoveryDuration = 0.3f; // Duración de la recarga

    // Rotaciones inicial y final del puñetazo
    private Vector3 punchStartRotation = new Vector3(22.2278385f, 29.7395287f, 56.4893761f);
    private Vector3 punchEndRotation = new Vector3(320.666016f, 264.120972f, 93.7342072f);

    void Start()
    {
        characterGeneralController = GetComponent<CharacterGeneralController>();

        if (fist == null)
        {
            Debug.LogError("No se encontró el componente Fist en los hijos.");
        }
    }

    public void PunchAttack()
    {
        if (!isPunching)
        {
            StartCoroutine(PunchCoroutine());
        }
    }

    private IEnumerator PunchCoroutine()
    {
        isPunching = true;
        characterGeneralController.SetStates(!isPunching);

        // Activar el collider del puño antes de comenzar el golpe
        fist.EnableCollider(true);

        // Guarda la rotación inicial del brazo derecho
        Quaternion originalRotation = rightArm.localRotation;

        // Calcula las rotaciones inicial y final del puñetazo
        Quaternion startRotation = Quaternion.Euler(punchStartRotation);
        Quaternion endRotation = Quaternion.Euler(punchEndRotation);

        // Realiza el puñetazo (fase rápida)
        for (float t = 0; t < punchDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / punchDuration;
            rightArm.localRotation = Quaternion.Slerp(startRotation, endRotation, normalizedTime);
            yield return null;  // Espera al siguiente frame
        }

        // Espera el tiempo de recarga antes de regresar el brazo a su posición original
        yield return new WaitForSeconds(recoveryDuration);

        // Desactivar el collider del puño al finalizar el golpe
        fist.EnableCollider(false);

        // Regresa el brazo a su posición original
        rightArm.localRotation = originalRotation;

        isPunching = false;
        characterGeneralController.SetStates(!isPunching);
    }
}
