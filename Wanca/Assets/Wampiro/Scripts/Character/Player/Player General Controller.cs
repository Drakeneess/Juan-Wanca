using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGeneralController : CharacterGeneralController
{
    
    private Pointer pointer;
    private CharacterRotation rotation;
    private ItemController itemController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  // Llamamos al Start de la clase base para inicializar correctamente los componentes generales.
        pointer = GetComponentInChildren<Pointer>();  // Obtener el componente Pointer en el jugador.
        rotation = GetComponent<CharacterRotation>();  // Obtener el componente de rotación del jugador.
        itemController = GetComponent<ItemController>();  // Obtener el componente de control de items del jugador
        shakeMagnitude=1f;
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
    }

    private void Update() {
        if(transform.position.y<-200){
            Death();
        }
    }
    // Método adicional para manejar la activación de componentes específicos del jugador.
    public override void ActivateComponents(bool state)
    {
        base.ActivateComponents(state); // Llamada opcional si tienes lógica en el padre.
        if (pointer != null)
        {
            pointer.SetPointerActive(state); // Activa o desactiva el puntero.
        }
    }

    public void Respawn(){
        // Aquí puedes implementar la lógica para que el jugador se resucite.
        SetStates(true);
    }
    public override void Death()
    {
        base.Death();
        cam.ShakeCamera(3f);
        enabled=false;
    }
    public void Cure(){
        if(actualHealth<maxHealth){
            actualHealth++;
        }
        else{
            Death();
        }
    }
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
    protected override void OnEnemyContact()
    {
        base.OnEnemyContact();
        TakeDamage(1);
    }
}
