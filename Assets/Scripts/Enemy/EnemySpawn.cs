using UnityEngine;

using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject alternateEnemyPrefab;
    public GameObject bossPrefab; // Prefab del jefe
    public Transform player;

    public float initialSpawnInterval = 2f;
    public int initialMaxEnemies = 10;
    public Rect spawnArea;

    private int currentEnemyCount = 0;
    private float currentSpawnInterval;
    private float healthMultiplier = 1f;
    private float enemySpeedMultiplier = 1f;
    private bool isAlternateEnemyUnlocked = false;
    private bool isBossSpawned = false;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnEnemies());
        StartCoroutine(ProgressDifficulty());
        StartCoroutine(UnlockAlternateEnemy());
        StartCoroutine(SpawnBossAfterTime(120f));
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            if (currentEnemyCount < initialMaxEnemies && !isBossSpawned)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        float spawnX = Random.Range(spawnArea.xMin, spawnArea.xMax);
        float spawnY = Random.Range(spawnArea.yMin, spawnArea.yMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        if (!IsInView(spawnPosition))
        {
            GameObject enemyToSpawn = isAlternateEnemyUnlocked && Random.value > 0.9f
                ? alternateEnemyPrefab
                : enemyPrefab;

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            currentEnemyCount++;

            EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.SetHealthMultiplier(healthMultiplier);
            }
            EnemyBehavior enemyBehavior = newEnemy.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.SetSpeedMultiplier(enemySpeedMultiplier);
            }
        }
    }

    bool IsInView(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        return viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1;
    }

    private IEnumerator ProgressDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            enemySpeedMultiplier += 0.5f;
            healthMultiplier += 0.2f;
            currentSpawnInterval = Mathf.Max(0.5f, currentSpawnInterval * 0.5f);

            Debug.Log($"Dificultad incrementada: Salud x{healthMultiplier}, Spawn Intervalo: {currentSpawnInterval}");
        }
    }

    private IEnumerator UnlockAlternateEnemy()
    {
        yield return new WaitForSeconds(30f);
        isAlternateEnemyUnlocked = true;
        Debug.Log("¡Enemigo alternativo desbloqueado!");
    }

    private IEnumerator SpawnBossAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (!isBossSpawned)
        {
            SpawnBoss();
            isBossSpawned = true;
        }
    }

    private void SpawnBoss()
    {
        Vector3 spawnPosition = new Vector3(player.position.x + 10f, player.position.y + 10f, 0); // Ejemplo: aparece cerca del jugador

        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("¡El jefe ha aparecido!");
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
