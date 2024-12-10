using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossController : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        AttackA,
        AttackB
    }

    public BossState currentState = BossState.Idle;

    public float maxHealth = 5000f;
    public string playerTag = "Player";
    public GameObject projectilePrefab;
    public float attackInterval = 2f;

    public float orbitSpeed = 50f;
    public float orbitRadius = 5f;
    public float orbitAmplitude = 1f;
    public float orbitFrequency = 2f;

    private Transform player;
    private float currentHealth;
    private bool isAttacking = false;
    private float orbitAngle = 0f;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Jugador no encontrado. Asegúrate de que el objeto tiene el Tag 'Player'.");
        }

        currentHealth = maxHealth;
        StartCoroutine(StateMachine());
    }

    void Update()
    {
        if (player == null) return;

        HandleOrbitalMovement();
    }

    void HandleOrbitalMovement()
    {
        if (player == null) return;

        orbitAngle += orbitSpeed * Time.deltaTime;

        float angleRad = orbitAngle * Mathf.Deg2Rad;

        float dynamicRadius = orbitRadius + Mathf.Sin(Time.time * orbitFrequency) * orbitAmplitude;

        Vector3 offset = new Vector3(
            Mathf.Cos(angleRad) * dynamicRadius,
            Mathf.Sin(angleRad) * dynamicRadius,
            0f
        );

        transform.position = player.position + offset;
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    HandleIdleState();
                    break;
                case BossState.AttackA:
                    HandleAttackAState();
                    break;
                case BossState.AttackB:
                    HandleAttackBState();
                    break;
            }
            yield return null;
        }
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
        Debug.Log($"Cambiando al estado: {newState}");
    }

    void HandleIdleState()
    {
        Debug.Log("Estado Idle: Esperando...");
        if (!isAttacking && Random.value > 0.9f)
        {
            ChangeState(Random.value > 0.5f ? BossState.AttackA : BossState.AttackB);
        }
    }

    void HandleAttackAState()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Debug.Log("Estado AttackA: Disparando proyectiles.");
            StartCoroutine(ShootProjectiles(5, 0.3f, BossState.Idle));
        }
    }

    void HandleAttackBState()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Debug.Log("Estado AttackB: Ataque circular.");
            StartCoroutine(ShootCircularProjectiles(8, 1f, BossState.Idle));
        }
    }

    IEnumerator ShootProjectiles(int count, float interval, BossState nextState)
    {
        for (int i = 0; i < count; i++)
        {
            if (player == null) yield break;
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f;
            yield return new WaitForSeconds(interval);
        }
        isAttacking = false;
        ChangeState(nextState);
    }

    IEnumerator ShootCircularProjectiles(int count, float interval, BossState nextState)
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < 360; j += 360 / count)
            {
                float angle = Mathf.Deg2Rad * j;
                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = direction * 3f;
            }
            yield return new WaitForSeconds(interval);
        }
        isAttacking = false;
        ChangeState(nextState);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Jefe: Recibió daño, salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("¡El jefe ha sido derrotado!");
        Destroy(gameObject);
        SceneManager.LoadScene("Victory");
    }
}
