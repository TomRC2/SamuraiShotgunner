using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthShield : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxShield = 100f;
    public float shieldRegenRate = 5f;
    public float shieldRegenDelay = 3f;
    public Scrollbar healthBar;
    public Scrollbar shieldBar;

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
                TakeDamage(damageValues[i]);
                break;
            }
        }
    }

    public void TakeDamage(float damage)
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

        if (currentHealth < 0)
        {
            currentHealth = 0;
            Debug.Log("El jugador ha muerto.");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthBar.size = currentHealth / maxHealth;
        shieldBar.size = currentShield / maxShield;
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
