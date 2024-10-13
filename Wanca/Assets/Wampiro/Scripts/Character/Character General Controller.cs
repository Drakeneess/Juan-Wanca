using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGeneralController : MonoBehaviour
{
    public float maxHealth=5;
    public ParticleSystem explosion;
    public float damage = 1f;
    public LayerMask characterLayer;

    protected float actualHealth;
    protected CharacterAnimations animations; // Acceso protegido para usarlo en clases derivadas.
    protected MovingCharacter movement; // Acceso protegido para usarlo en clases derivadas.
    protected bool damageable=true;
    protected CameraController cam;
    protected float shakeMagnitude=0.5f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        actualHealth=maxHealth;

        cam=Camera.main.GetComponent<CameraController>();
        animations = GetComponent<CharacterAnimations>();
        movement = GetComponent<MovingCharacter>();
        explosion=GetComponentInChildren<ParticleSystem>();
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
        cam.ShakeCamera(shakeMagnitude);
        SetStates(false);
        ExplosionControl();
        GetComponent<Fragmentation>().DestroyMesh();
    }
    public virtual void TakeDamage(float damage){
        if (damageable) {
            // Manejar el daño recibido por el jugador.
            actualHealth -= damage;
            if (actualHealth <= 0)
            {
                Death();
            }
        }
    }
    protected virtual void OnCollisionEnter(Collision other) {
        // Manejar la colisión con un objeto.
        if (other.gameObject.layer == characterLayer) {
            if(other.gameObject.tag=="Player"){
                // Manejar la colisión con el jugador.
                OnPlayerContact();
            }
            else if(other.gameObject.tag=="Enemy"){
                // Manejar la colisión con un enemigo.
                OnEnemyContact();
            }
        }
    }

    protected virtual void OnPlayerContact(){

    }

    protected  virtual void OnEnemyContact(){
        
    }


    private void ExplosionControl(){
        Destroy(explosion,2f);
        explosion.transform.SetParent(null);
        explosion.Play();
    }
    public void SetDamageableStatus(bool state){
        damageable = state;
    }
    public float GetHealth(){
        return actualHealth;
    }
}
