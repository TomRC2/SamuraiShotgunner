using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject experienceOrbPrefab;
    public Scrollbar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
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
            healthBar.size = currentHealth / maxHealth;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            TakeDamage(20f);
        }
        else if (other.CompareTag("Bullet"))
        {
            TakeDamage(2f);
            Destroy(other.gameObject);
        }
    }
    public void Die()
    {
        
        Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}

