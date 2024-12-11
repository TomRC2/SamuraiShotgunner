using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public int experienceAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().AddExperience(experienceAmount);
            Destroy(gameObject);
        }
    }
}
