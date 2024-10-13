using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab; // Prefab del enemigo a spawn
        public float spawnChance;       // Probabilidad de spawn del enemigo
    }

    public List<EnemySpawnInfo> enemiesToSpawn; // Lista de enemigos y sus probabilidades
    public Transform player;           // Referencia al transform del jugador
    public float spawnInterval = 5f;   // Intervalo entre spawns
    public int maxEnemiesToSpawn = 10; // Máximo de enemigos a spawn

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Inicializa la referencia del jugador
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (GetActiveEnemyCount() < maxEnemiesToSpawn)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval); // Esperar el intervalo de spawn
        }
    }

    private void SpawnEnemy()
    {
        // Calcular la probabilidad total
        float totalChance = 0f;
        foreach (var enemy in enemiesToSpawn)
        {
            totalChance += enemy.spawnChance;
        }

        // Generar un número aleatorio para determinar qué enemigo spawnear
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var enemy in enemiesToSpawn)
        {
            cumulativeChance += enemy.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                // Encontrar una posición válida en el NavMesh
                Vector3 spawnPosition = GetRandomNavMeshPosition();
                if (spawnPosition != Vector3.zero)
                {
                    GameObject newEnemy = Instantiate(enemy.enemyPrefab, spawnPosition, Quaternion.identity);

                    // Obtener el componente del enemigo y asignar la referencia del jugador
                    EnemyMortar enemyScript = newEnemy.GetComponent<EnemyMortar>();
                    if (enemyScript != null)
                    {
                        enemyScript.SetPlayer(player); // Asignar el jugador al enemigo
                    }
                }
                break;
            }
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        float spawnRadius = 20f; // Radio en el que se buscará una posición de spawn

        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += player.position; // Centrar alrededor del jugador

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position; // Devuelve una posición válida en el NavMesh
        }

        return Vector3.zero; // Retorna cero si no encuentra una posición válida
    }

    private int GetActiveEnemyCount()
    {
        return FindObjectsOfType<Enemy>().Length; // Cambia 'Enemy' por el nombre de tu script de enemigo
    }
}

