using System.Collections;
using UnityEngine;

public class PlayerGeneralController : CharacterGeneralController,IDamageable
{
    public GameObject deathMenu;
    private Pointer pointer;
    private CharacterRotation rotation;
    private ItemController itemController;
    private AttackController attackController;
    
    public float invulnerabilityDuration = 2f; // Duración de la ventana de invulnerabilidad.
    private bool isInvulnerable = false; // Variable para verificar si el jugador es invulnerable.

    protected override void Start()
    {
        base.Start();  // Llamamos al Start de la clase base para inicializar correctamente los componentes generales.
        pointer = GetComponentInChildren<Pointer>();  // Obtener el componente Pointer en el jugador.
        rotation = GetComponent<CharacterRotation>();  // Obtener el componente de rotación del jugador.
        itemController = GetComponent<ItemController>();  // Obtener el componente de control de items del jugador.
        attackController = GetComponent<AttackController>();
        shakeMagnitude = 1f;
    }

    // Método que sobrescribe la activación/desactivación de los estados.
    public override void SetStates(bool state)
    {
        base.SetStates(state); // Llamamos al método base para manejar las animaciones.
        if (pointer != null)
        {
            pointer.SetPointerActive(state); // Activar o desactivar el puntero.
        }
        if(rotation != null){
            rotation.SetRotationState(state); // Activar o desactivar la rotación.
        }
        if(itemController !=  null){
            itemController.SetItemAwayState(state); // Activar o desactivar el controlador de
        }
        if(attackController != null){
            attackController.SetCanAttacking(state); // Activar o desactivar el controlador de at
        }
    }

    public override void TakeDamage(float damage)
    {
        if (!isInvulnerable)  // Solo recibir daño si no está en estado de invulnerabilidad.
        {
            base.TakeDamage(damage);
            StartCoroutine(InvulnerabilityWindow());  // Iniciar la ventana de invulnerabilidad.
        }
    }

    private IEnumerator InvulnerabilityWindow()
    {
        isInvulnerable = true;  // Hacer al jugador invulnerable.
        SetDamageableStatus(false);  // Desactivar la capacidad de recibir daño.

        yield return new WaitForSeconds(invulnerabilityDuration);  // Esperar el tiempo de invulnerabilidad.

        isInvulnerable = false;  // Finaliza la invulnerabilidad.
        SetDamageableStatus(true);  // Reactivar la capacidad de recibir daño.
    }

    protected override void OnEnemyContact()
    {
        base.OnEnemyContact();
        TakeDamage(1);
    }

    private void Update()
    {
        if (transform.position.y < -200)
        {
            Death();
        }
    }

    public void Cure()
    {
        if (actualHealth < maxHealth)
        {
            actualHealth++;
        }
        else
        {
            Death();
        }
    }
    public override void Death()
    {
        base.Death();
        cam.ShakeCamera(3f);
        enabled=false;
        DeathScreen();
    }

    private void DeathScreen(){
        // Aquí puedes agregar la lógica para mostrar la pantalla de muerte.
        GameObject canvas=GameObject.FindWithTag("Main Canvas");
        GameObject menu = Instantiate(deathMenu);
        menu.transform.SetParent(canvas.transform, false);
        menu.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void TakeDamage(int Damage)
    {
        actualHealth -= Damage;

        if (actualHealth <= 0)
        {
            Death();
        }
    }

    public Transform GetTransform()
    {
        return transform;

    }
}
