using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceWeapon : Weapon
{
    public Projectile[] projectiles;  // Prefabs de proyectiles asignados en el Inspector
    public GameObject[] WeaponUI;     // Prefabs/UI para el arma
    protected float fireRate = 0.5f;  // Tiempo entre disparos

    private enum WeaponType
    {
        Shotgun,
        Handgun
    }

    private WeaponType type;                   // Tipo de arma
    protected Projectile weaponProjectile;     // Proyectil asignado
    public GameObject weaponUI;
    protected float nextFireTime = 0f;         // Tiempo hasta el próximo disparo
    protected bool canShoot = true;            // Bandera para controlar si se puede disparar

    protected WeaponUIController weaponUIController;

    // Start is called before the first frame update
    void Start()
    {
        weaponUIController = FindObjectOfType<WeaponUIController>();  // Encontrar el controlador de la UI
        SetRandomType();  // Asignar tipo de arma aleatorio
    }

    // Establecer un tipo de arma aleatorio
    private void SetRandomType()
    {
        type = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);

        switch (type)
        {
            case WeaponType.Shotgun:
                var shotgun = gameObject.AddComponent<Shotgun>();
                shotgun.SetProjectileAndUI(0, projectiles, WeaponUI);  // Asumimos que el proyectil 0 es para shotgun
                SetProjectileAndUI(0, projectiles, WeaponUI); // Actualizar UI y proyectil
                break;

            case WeaponType.Handgun:
                var handgun = gameObject.AddComponent<Handgun>();
                handgun.SetProjectileAndUI(1, projectiles, WeaponUI);  // Asumimos que el proyectil 1 es para handgun
                SetProjectileAndUI(1, projectiles, WeaponUI); // Actualizar UI y proyectil
                break;
        }

        Destroy(this); // Destruye el componente original de DistanceWeapon
    }

    protected virtual void Update()
    {
        // Si se ha alcanzado el tiempo de espera, se puede disparar
        if (Time.time >= nextFireTime)
        {
            canShoot = true; // Permitir disparo
        }
    }

    public override void Attack()
    {
        if (!canShoot) { return; } // Si no se puede disparar, salir

        base.Attack(); // Llama al método Attack de la clase base
        Shoot(); // Dispara el proyectil
        canShoot = false; // No permitir otro disparo hasta que pase el tiempo de espera
        nextFireTime = Time.time + fireRate; // Actualiza el tiempo del próximo disparo
    }

    // Método para disparar el proyectil instanciado desde el prefab
    protected virtual void Shoot()
    {
        // Lógica de disparo personalizada, si es necesario
        if (weaponProjectile != null)
        {
            Instantiate(weaponProjectile, transform.position, transform.rotation); // Instancia el proyectil
        }
    }

    // Método para asignar el prefab del proyectil correspondiente y actualizar la UI
    protected virtual void SetProjectileAndUI(int index, Projectile[] baseProjectiles, GameObject[] baseUIWeapon)
    {
        // Asegurarse de que el array de prefabs no sea null y el índice esté dentro de los límites
        if (baseProjectiles != null && index >= 0 && index < baseProjectiles.Length)
        {
            weaponProjectile = baseProjectiles[index];

            // Asegurarse de que el controlador de UI y el prefab de la UI existen
            if (baseUIWeapon != null && index < baseUIWeapon.Length)
            {
                weaponUI = baseUIWeapon[index];
            }
            else
            {
                Debug.LogWarning("WeaponUIController o WeaponUI no están correctamente configurados.");
            }

            if (weaponProjectile == null)
            {
                Debug.LogWarning("El prefab del proyectil en el índice " + index + " no está asignado.");
            }
        }
        else
        {
            Debug.LogWarning("El array de proyectiles no está inicializado o el índice está fuera de rango.");
        }
    }

    public override void Action(Transform player)
    {
        base.Action(player);
        weaponUIController.SetWeaponUI(weaponUI);
    }
}
