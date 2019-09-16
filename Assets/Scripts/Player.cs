using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed;
    public Transform groundCheck;
    public float jumpStrength;
    public LayerMask ladderLayer;
    public GameObject basicShotPrefab;
    public Transform shootingTrigger; 

    private bool isGrounded = false;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool isClimbing = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private new Rigidbody2D rigidbody2D;
    private float defaultGravityScale;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultGravityScale = rigidbody2D.gravityScale;
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

        if (Input.GetButtonDown("Fire1"))
        {
            this.Fire();
        }

    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        rigidbody2D.velocity = new Vector2(direction * walkSpeed, rigidbody2D.velocity.y);

        if ((direction < 0f && isFacingRight) || (direction > 0f && !isFacingRight))
            this.Flip();

        if (isJumping)
        {
            rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, jumpStrength));
            isJumping = false;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.up, 10, ladderLayer);
        Debug.DrawRay(groundCheck.position, Vector2.up, Color.red);

        if (hitInfo.collider != null)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                isClimbing = true;
                transform.position = new Vector2(hitInfo.transform.position.x, transform.position.y);
            }else if (Input.GetButtonDown("Jump") || (isGrounded && Input.GetAxisRaw("Horizontal") != 0))
            {
                isClimbing = false;
            }

        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing && hitInfo.collider != null)
        {
            float vDirection = Input.GetAxisRaw("Vertical");
            animator.speed = vDirection == 0 ? 0 : 1;

            rigidbody2D.velocity = new Vector2(0f, vDirection * 3);
            rigidbody2D.gravityScale = 0;
        }
        else
        {
            animator.speed = 1;
            rigidbody2D.gravityScale = defaultGravityScale;
        }
    }

    private void Animations()
    {
        animator.SetBool("isWalking", isGrounded && rigidbody2D.velocity.x != 0f);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetInteger("yVel", (int)rigidbody2D.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        base.transform.Rotate(0f, 180, 0f);
    }

    private void Fire()
    {
        animator.SetTrigger("Fire");
        Instantiate(basicShotPrefab, shootingTrigger.position, shootingTrigger.rotation);   
    }
}
