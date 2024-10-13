using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIController : MonoBehaviour
{
    private GameObject weaponUI;

    // Método para establecer el nuevo UI del arma
    public void SetWeaponUI(GameObject newWeaponUI)
    {
        if (weaponUI != null)
        {
            Destroy(weaponUI); // Destruye la UI anterior si ya existe
        }

        // Instancia la nueva UI y mantiene su rotación original
        weaponUI = Instantiate(newWeaponUI, transform.position, newWeaponUI.transform.rotation); 

        weaponUI.transform.SetParent(transform); // Asegura que la UI esté dentro del WeaponUIController
        weaponUI.transform.localPosition = Vector3.zero; // Resetea la posición local para que esté bien posicionada

        // Mantener la rotación original
        weaponUI.transform.localRotation = newWeaponUI.transform.localRotation;
    }
}
