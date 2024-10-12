using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Action(Transform player)
    {
        base.Action(player);
        AttackController attackController = player.GetComponent<AttackController>();
        if(attackController != null){
            Transform weaponHolder = attackController.weaponHolder;
            transform.SetParent(weaponHolder);
            transform.position = weaponHolder.position;
            transform.rotation = weaponHolder.rotation;

            attackController.SetWeapon(this);
        }
    }
    public virtual void Attack(){
        
    }
}
