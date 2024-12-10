using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 5000f;
    private float currentHealth;

    public Slider healthBar;
    private BossController bossController;
    private Player player;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        bossController = GetComponent<BossController>();
        if (bossController == null)
        {
            Debug.LogError("No se encontró el script BossController en el jefe.");
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Jugador no encontrado. Asegúrate de que el objeto tiene el Tag 'Player'.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        Debug.Log($"Jefe: Recibió {damage} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0 && bossController != null)
        {
            bossController.Die();
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
            if (player != null)
            {
                TakeDamage(player.baseDamage);
            }
        }
        else if (other.CompareTag("Bullet"))
        {
            if (player != null)
            {
                TakeDamage(player.bulletDamage);
            }
            Destroy(other.gameObject);
        }
    }
}
