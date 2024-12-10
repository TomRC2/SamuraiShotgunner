using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Importa el espacio de nombres

public class PlayerHealthShield : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxShield = 100f;
    public float shieldRegenRate = 5f;
    public float shieldRegenDelay = 3f;
    public Slider healthBar;
    public Slider shieldBar;

    public string[] damageTags;
    public float[] damageValues;

    private float currentHealth;
    private float currentShield;
    private float shieldRegenTimer;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;

        UpdateUI();
    }

    void Update()
    {
        if (currentShield < maxShield)
        {
            shieldRegenTimer += Time.deltaTime;

            if (shieldRegenTimer >= shieldRegenDelay)
            {
                currentShield += shieldRegenRate * Time.deltaTime;
                if (currentShield > maxShield)
                    currentShield = maxShield;

                UpdateUI();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < damageTags.Length; i++)
        {
            if (other.CompareTag(damageTags[i]))
            {
                TakeDamages(damageValues[i]);
                break;
            }
        }
    }

    public void TakeDamages(float damage)
    {
        shieldRegenTimer = 0;

        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield < 0)
            {
                currentHealth += currentShield;
                currentShield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("El jugador ha muerto.");
            SceneManager.LoadScene("Lose"); 
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.value = currentHealth / maxHealth;
        shieldBar.value = currentShield / maxShield;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateUI();
    }

    public void RechargeShield(float amount)
    {
        currentShield += amount;
        if (currentShield > maxShield)
            currentShield = maxShield;

        UpdateUI();
    }
}
