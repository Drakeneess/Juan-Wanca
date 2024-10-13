using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : DistanceWeapon
{
    public int pelletsCount = 10; // Número de perdigones a disparar
    public float spreadAngle = 15f; // Ángulo de dispersión en grados

    // Start is called before the first frame update
    void Start()
    {
        weaponUIController = FindObjectOfType<WeaponUIController>();  // Encontrar el controlador de la UI

        fireRate = 0.6f; // Establecer el tiempo entre disparos
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Shoot()
    {
        base.Shoot();

        // Disparar varios perdigones
        for (int i = 0; i < pelletsCount; i++)
        {
            // Calcular la dirección con dispersión
            float angle = Random.Range(-spreadAngle, spreadAngle); // Ángulo aleatorio para dispersión
            Quaternion spreadRotation = Quaternion.Euler(0, angle, 0); // Rotación en el eje Y
            Vector3 direction = spreadRotation * transform.forward; // Aplicar la rotación a la dirección de disparo

            // Instanciar el proyectil en la posición y rotación
            Projectile pellet = Instantiate(weaponProjectile, transform.position, Quaternion.LookRotation(direction));
            // Opcionalmente puedes configurar la velocidad del proyectil aquí
        }
    }
}
