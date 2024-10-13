using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int poolSize = 10;

    private Queue<MortarProjectile> pool = new Queue<MortarProjectile>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            MortarProjectile projectile = Instantiate(projectilePrefab).GetComponent<MortarProjectile>();
            projectile.gameObject.SetActive(false);
            pool.Enqueue(projectile);
        }
    }

    public MortarProjectile GetProjectile()
    {
        if (pool.Count > 0)
        {
            MortarProjectile projectile = pool.Dequeue();
            projectile.gameObject.SetActive(true);
            return projectile;
        }
        return null; // O instanciar un nuevo proyectil si lo deseas
    }

    public void ReturnProjectile(MortarProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        pool.Enqueue(projectile);
    }
}

