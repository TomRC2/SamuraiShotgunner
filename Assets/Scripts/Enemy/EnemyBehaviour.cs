using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float baseMoveSpeed = 3f;
    public float speedMultiplier = 1f;
    public string playerTag = "Player";

    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) return;
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * baseMoveSpeed * speedMultiplier * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        rb.MovePosition(rb.position + movement * GetCurrentSpeed() * Time.fixedDeltaTime);
    }

    private float GetCurrentSpeed()
    {
        return baseMoveSpeed * speedMultiplier;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}
