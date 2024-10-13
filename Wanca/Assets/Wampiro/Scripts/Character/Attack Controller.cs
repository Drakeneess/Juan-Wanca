using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform weaponHolder;
    private Weapon weapon;
    private Punch punch;
    private WeaponUIController weaponUIController;

    // Start is called before the first frame update
    void Start()
    {
        weaponUIController = FindAnyObjectByType<WeaponUIController>();
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
    public Weapon ActualWeapon(){
        return weapon;
    }
    public void DropWeapon()
    {
        if (weapon != null)
        {
            // Soltar el arma del weaponHolder
            weapon.transform.SetParent(null); // Desvincula el arma del weaponHolder
            
            Destroy(weapon.gameObject);
            weaponUIController.UnsetWeaponUI();

            weapon = null; // Eliminar la referencia al arma actual en el AttackController
        }
    }
}
