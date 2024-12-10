using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public int experienceAmount = 10; // Cantidad de experiencia que otorga la orbe

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Llama al método para otorgar experiencia al jugador
            collision.GetComponent<Player>().AddExperience(experienceAmount);
            // Destruye la orbe
            Destroy(gameObject);
        }
    }
}