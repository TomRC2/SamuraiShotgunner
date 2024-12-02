using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public LayerMask grassLayer; 
    public float spawnRadius = 10f; 
    public float spawnCheckRadius = 0.5f; 
    public float spawnInterval = 3f; 

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        int attempts = 10;

        do
        {
            attempts--;
            spawnPosition = GetRandomPosition();

            if (attempts <= 0)
            {
                Debug.LogWarning("No se encontró una posición válida para spawnear un enemigo.");
                return;
            }
        }
        while (!IsPositionValid(spawnPosition));

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
        randomOffset.z = 0;
        return player.position + randomOffset;
    }

    bool IsPositionValid(Vector3 position)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
        {
            return false;
        }

        Collider2D hit = Physics2D.OverlapCircle(position, spawnCheckRadius, grassLayer);
        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnRadius);
    }
}
