using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGeneralController : MonoBehaviour
{
    public float maxHealth=5;

    protected float actualHealth;
    protected CharacterAnimations animations; // Acceso protegido para usarlo en clases derivadas.
    protected MovingCharacter movement; // Acceso protegido para usarlo en clases derivadas.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        actualHealth=maxHealth;

        animations = GetComponent<CharacterAnimations>();
        movement = GetComponent<MovingCharacter>();
    }

    // Método para activar o desactivar el estado del personaje.
    public virtual void SetStates(bool state)
    {
        if (animations != null)
        {
            animations.SetAnimate(state);
            movement.SetMoveState(state);
        }
    }

    // Método para activar algún componente adicional en el personaje.
    public virtual void ActivateComponents(bool state)
    {
        // Aquí podrías implementar lógica para otros componentes genéricos, si es necesario.
    }
    public virtual void Death(){
        // Manejar la muerte del jugador.
        SetStates(false);
    }
    public virtual void  TakeDamage(float damage){
        // Manejar el daño recibido por el jugador.
        actualHealth -= damage;
        if (actualHealth <= 0)
        {
            Death();
        }
    }
}
