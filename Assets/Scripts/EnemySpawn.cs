using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public LayerMask grassLayer; // Capa de "Grass"
    public float spawnRadius = 10f; // Distancia mínima para spawnear enemigos fuera de la cámara
    public float spawnCheckRadius = 0.5f; // Radio para verificar si está sobre "Grass"
    public float spawnInterval = 3f; // Tiempo entre spawns

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval); // Llama al método repetidamente
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        int attempts = 10; // Intentos para encontrar una posición válida

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
        Vector3 randomOffset = Random.insideUnitCircle * spawnRadius; // Posición aleatoria dentro de un radio
        randomOffset.z = 0; // Aseguramos que esté en 2D
        return player.position + randomOffset;
    }

    bool IsPositionValid(Vector3 position)
    {
        // Verifica si está fuera de la cámara
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
        {
            return false; // Está dentro de la cámara
        }

        // Verifica si está sobre "Grass"
        Collider2D hit = Physics2D.OverlapCircle(position, spawnCheckRadius, grassLayer);
        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnRadius); // Dibuja el radio de spawneo
    }
}
