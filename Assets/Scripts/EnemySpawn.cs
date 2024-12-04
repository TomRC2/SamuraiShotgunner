using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;         // El tipo inicial de enemigo
    public GameObject alternateEnemyPrefab; // El nuevo tipo de enemigo que aparecerá después de 30 segundos
    public Transform player;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;
    public Rect spawnArea;

    private int currentEnemyCount = 0;
    private bool isAlternateEnemyActive = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
        StartCoroutine(SwitchEnemyType()); // Inicia la Coroutine para cambiar el tipo de enemigo
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        float spawnX = Random.Range(spawnArea.xMin, spawnArea.xMax);
        float spawnY = Random.Range(spawnArea.yMin, spawnArea.yMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        if (!IsInView(spawnPosition))
        {
            // Si ya han pasado 30 segundos, spawnea el nuevo tipo de enemigo
            GameObject enemyToSpawn = isAlternateEnemyActive ? alternateEnemyPrefab : enemyPrefab;
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            currentEnemyCount++;
        }
    }

    bool IsInView(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        return viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }

    // Coroutine que cambia el tipo de enemigo después de 30 segundos
    private IEnumerator SwitchEnemyType()
    {
        yield return new WaitForSeconds(30f); // Espera 30 segundos
        isAlternateEnemyActive = true; // Cambia al tipo alternativo de enemigo
    }
}
