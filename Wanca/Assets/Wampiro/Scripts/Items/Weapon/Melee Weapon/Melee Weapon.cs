using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public Vector3 initialPosition;
    public Vector3 endPosition;

    private enum WeaponType
    {
        DoubleSaber,
        Sword,
        Hammer,
        Knife
    }
    private WeaponType type;                   // Tipo de arma
    public GameObject weaponUI;
    

    
    public  float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
