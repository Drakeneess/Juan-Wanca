using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform weaponHolder;
    private Weapon weapon;
    private Punch punch;

    // Start is called before the first frame update
    void Start()
    {
        punch=GetComponent<Punch>();
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
            punch.PunchAttack();
        }
    }
    public void SetWeapon(Weapon newWeapon){
        weapon = newWeapon;
    }
}
