using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer playerRenderer;
    private Vector2 movement;

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        animator.SetFloat("Movement", movement.x);
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