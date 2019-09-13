using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed;
    public Transform groundCheck;
    public float jumpStrength;

    private bool isGrounded = false;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private new Rigidbody2D rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        isGrounded = Physics2D.Linecast(base.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        this.Animations();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(direction * walkSpeed, rigidbody2D.velocity.y);

        if ((direction < 0f && isFacingRight) || (direction > 0f && !isFacingRight))
            this.Flip();

        if (isJumping)
        {
            rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, jumpStrength));
            isJumping = false;
        }
    }

    private void Animations()
    {
        animator.SetBool("isWalking", isGrounded && rigidbody2D.velocity.x != 0f);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        base.transform.localScale = new Vector3(-base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
    }
}
