using UnityEngine;

public class EnemySniper : MonoBehaviour
{
    [Header("Movement Settings")]
    private Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 5f; 

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 10f; 

    private float nextFireTime = 0f;
    public string playerTag = "Player";

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
    void Update()
    {
        MoveTowardsPlayer();

        if (Vector2.Distance(transform.position, player.position) <= stopDistance)
        {
            ShootAtPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void ShootAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}
