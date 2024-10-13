using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public GameObject weaponUIPrefab;

    private WeaponUIController weaponUIController;

    // Start is called before the first frame update
    void Start()
    {
        weaponUIController = FindObjectOfType<WeaponUIController>();
    }

    public void SetWeaponUI(GameObject newWeaponUI)
    {
        if (weaponUIController != null)
        {
            weaponUIController.SetWeaponUI(newWeaponUI);
        }
    }
}
