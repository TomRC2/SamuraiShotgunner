using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public LayerMask grassLayer; // Capa de "Grass"
    public float spawnRadius = 10f; // Distancia m�nima para spawnear enemigos fuera de la c�mara
    public float spawnCheckRadius = 0.5f; // Radio para verificar si est� sobre "Grass"
    public float spawnInterval = 3f; // Tiempo entre spawns

    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval); // Llama al m�todo repetidamente
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        int attempts = 10; // Intentos para encontrar una posici�n v�lida

        do
        {
            attempts--;
            spawnPosition = GetRandomPosition();

            if (attempts <= 0)
            {
                Debug.LogWarning("No se encontr� una posici�n v�lida para spawnear un enemigo.");
                return;
            }
        }
        while (!IsPositionValid(spawnPosition));

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomOffset = Random.insideUnitCircle * spawnRadius; // Posici�n aleatoria dentro de un radio
        randomOffset.z = 0; // Aseguramos que est� en 2D
        return player.position + randomOffset;
    }

    bool IsPositionValid(Vector3 position)
    {
        // Verifica si est� fuera de la c�mara
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
        {
            return false; // Est� dentro de la c�mara
        }

        // Verifica si est� sobre "Grass"
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
