using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;
    public Rect spawnArea;

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        float spawnX = Random.Range(spawnArea.xMin, spawnArea.xMax);
        float spawnY = Random.Range(spawnArea.yMin, spawnArea.yMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        if (!IsInView(spawnPosition))
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
}