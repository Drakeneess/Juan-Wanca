using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceWeapon : Weapon
{
    public Projectile[] projectiles;  // Prefabs de proyectiles asignados en el Inspector
    private enum WeaponType
    {
        Shotgun,
        Handgun
    }
    private WeaponType type;  // Tipo de arma
    protected Projectile weaponProjectile;  // Proyectil asignado

    // Start is called before the first frame update
    void Start()
    {
        SetRandomType();
    }

    // Establecer un tipo de arma aleatorio
    private void SetRandomType()
    {
        type = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);

        switch (type)
        {
            case WeaponType.Shotgun:
                var shotgun = gameObject.AddComponent<Shotgun>();
                shotgun.SetProjectile(0, projectiles);  // Asumimos que el proyectil 0 es para shotgun
                break;

            case WeaponType.Handgun:
                var handgun = gameObject.AddComponent<Handgun>();
                handgun.SetProjectile(1, projectiles);  // Asumimos que el proyectil 1 es para handgun
                break;
        }

        Destroy(this);
    }

    public override void Attack()
    {
        base.Attack();
        Shoot();
    }

    // Método para disparar el proyectil instanciado desde el prefab
    protected virtual void Shoot()
    {
        
    }

    // Método para asignar el prefab del proyectil correspondiente
    protected virtual void SetProjectile(int index, Projectile[] baseProjectiles)
    {
        // Asegurarse de que el array de prefabs no sea null y el índice esté dentro de los límites
        if (baseProjectiles != null && index >= 0 && index < baseProjectiles.Length)
        {
            weaponProjectile = baseProjectiles[index];

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
}
