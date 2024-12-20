using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer playerRenderer;
    private Vector2 movement;

    private Player player;

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player != null)
        {
            moveSpeed = player.movementSpeed;
        }

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetBool("IsMoving", movement != Vector2.zero);

        ReflectPlayerSprite();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void ReflectPlayerSprite()
    {
        if (movement.x < 0)
        {
            playerRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            playerRenderer.flipX = false;
        }
    }
}
