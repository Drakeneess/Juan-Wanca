using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : DistanceWeapon
{
    
    // Start is called before the first frame update
    void Start()
    {
        weaponUIController = FindObjectOfType<WeaponUIController>();  // Encontrar el controlador de la UI

        fireRate=0.3f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void Shoot()
    {
        base.Shoot();
        Instantiate(weaponProjectile, transform.position, transform.rotation);
    }
}
