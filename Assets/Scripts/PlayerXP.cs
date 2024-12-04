using UnityEngine;

public class Player : MonoBehaviour
{
    public int experience = 0;
    public int level = 1;
    public int experienceToLevelUp = 100;
    public UnityEngine.UI.Scrollbar experienceBar;

    public void AddExperience(int amount)
    {
        experience += amount;
        UpdateExperienceBar();

        if (experience >= experienceToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToLevelUp;
        experienceToLevelUp += 50;
        Debug.Log("¡Nivel subido! Nivel actual: " + level);
    }

    private void UpdateExperienceBar()
    {
        if (experienceBar != null)
        {
            experienceBar.value = (float)experience / experienceToLevelUp;
        }
    }
}