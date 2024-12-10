using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject experienceOrbPrefab;
    public GameObject healthOrbPrefab;
    [Range(0f, 1f)] public float healthOrbDropChance = 0.2f;

    public Slider healthBar;

    private Player player;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            TakeDamage(player.baseDamage);
        }
        else if (other.CompareTag("Bullet"))
        {
            TakeDamage(player.bulletDamage);
            Destroy(other.gameObject);
        }
    }

    public void Die()
    {
        Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);

        if (healthOrbPrefab != null && Random.value < healthOrbDropChance)
        {
            Instantiate(healthOrbPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
        FindObjectOfType<EnemySpawner>().OnEnemyDestroyed();
    }
    public void SetHealthMultiplier(float multiplier)
    {
        maxHealth *= multiplier;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
}