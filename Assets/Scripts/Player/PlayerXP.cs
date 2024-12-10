using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int experience = 0;
    public int level = 1;
    public int experienceToLevelUp = 100;
    public Slider experienceBar;

    public float baseDamage = 20f;
    public float bulletDamage = 2f;
    public float movementSpeed = 5f;

    public Text statsText;
    public WeaponController weaponController;

    void Start()
    {
        UpdateExperienceBar();
        UpdateStatsText();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        UpdateExperienceBar();
        UpdateStatsText();

        while (experience >= experienceToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        while (experience >= experienceToLevelUp)
        {
            level++;
            experience -= experienceToLevelUp;
            experienceToLevelUp += 20;

            baseDamage += 5f;
            bulletDamage += 1f;
            movementSpeed += 0.2f;

            if (weaponController != null)
            {
                weaponController.shootCooldown = Mathf.Max(1.5f, weaponController.shootCooldown - 0.05f);

                if (level == 5)
                    weaponController.ChangeWeapon(1);
                else if (level == 10)
                    weaponController.ChangeWeapon(2);
                else if (level == 15)
                    weaponController.ChangeWeapon(3);
            }

            Debug.Log($"¡Nivel subido! Nivel actual: {level}");
        }

        UpdateExperienceBar();
        UpdateStatsText();
    }


    private void UpdateExperienceBar()
    {
        if (experienceBar != null)
        {
            experienceBar.value = (float)experience / experienceToLevelUp;
        }
    }

    private void UpdateStatsText()
    {
        if (statsText != null)
        {
            statsText.text =
                $"Nivel: {level}\n" +
                $"Experiencia: {experience}/{experienceToLevelUp}\n" +
                $"Daño Espada: {baseDamage:F1}\n" +
                $"Daño Balas: {bulletDamage:F1}\n" +
                $"Velocidad: {movementSpeed:F1}\n" +
                $"Cooldown Disparo: {(weaponController != null ? weaponController.shootCooldown.ToString("F2") : "N/A")}s";
        }
        else
        {
            Debug.LogWarning("El texto de estadísticas no está asignado en el Inspector.");
        }
    }
}
