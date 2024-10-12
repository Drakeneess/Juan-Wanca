using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform weaponHolder;
    private Weapon weapon;
    private CharacterAnimations characterAnimations;

    // Start is called before the first frame update
    void Start()
    {
        characterAnimations=GetComponent<CharacterAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack(){
        if(weapon!=null){
            weapon.Attack();
        }
        else{
            characterAnimations.Punch();
        }
    }
    public void SetWeapon(Weapon newWeapon){
        weapon = newWeapon;
    }
}
