using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SoundManager;

public class Player : MonoBehaviour
{
    public float walkSpeed;
    public Transform groundCheck;
    public float jumpStrength;
    public int health;
    public LayerMask ladderLayer;
    public GameObject basicShotPrefab;
    public Transform shootingTrigger;
    public AudioClip defaultShot;
    public AudioClip deathSound;
    public AudioClip landingSound;
    public AudioClip hurtSound;
    public AudioClip spawnSound;
    public SpriteRenderer hurtEffectSprite;

    private bool isGrounded = false;
    private bool inputEnable = true;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool isClimbing = false;
    private bool isSpawning = true;
    private bool isInvulnerable = true;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private new Rigidbody2D rigidbody2D;
    private float defaultGravityScale;
    private SoundManager soundManager;
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultGravityScale = rigidbody2D.gravityScale;
        soundManager = SoundManager.instance;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rigidbody2D.gravityScale = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        isGrounded = Physics2D.Linecast(base.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        this.Animations();

        if (Input.GetButtonDown("Jump") && isGrounded && inputEnable)
        {
            isJumping = true;
        }

        if (Input.GetButtonDown("Fire1") && !isSpawning && inputEnable)
        {
            this.Fire();
        }

    }

    private void FixedUpdate()
    {
        if (isSpawning)
        {
            if (isGrounded && (base.transform.position.y <= mainCamera.orthographicSize / 2))
            {
                this.InitializePlayer();
            }
            else
            {
                rigidbody2D.velocity = -base.transform.up * 35;
            }
            
        }
        else
        {
            if (animator.GetAnimatorTransitionInfo(0).IsName("Jumping -> Idle") || animator.GetAnimatorTransitionInfo(0).IsName("Jumping -> Walking"))
            {
                soundManager.PlaySound(ESource.Megaman, landingSound);
            }


            float direction = Input.GetAxisRaw("Horizontal");
            if (inputEnable)
            {
                rigidbody2D.velocity = new Vector2(direction * walkSpeed, rigidbody2D.velocity.y);
            }

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
                }
                else if (Input.GetButtonDown("Jump") || (isGrounded && Input.GetAxisRaw("Horizontal") != 0))
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
        soundManager.PlaySound(ESource.Shooting, defaultShot);
        animator.SetTrigger("Fire");
        Instantiate(basicShotPrefab, shootingTrigger.position, shootingTrigger.rotation);   
    }

    public void KillPlayer(bool toggleAnimation)
    {
        soundManager.StopMusic();
        soundManager.PlaySound(ESource.Megaman, deathSound);
        if (toggleAnimation)
        {
            // Toggle the death animation (blue ball in 8 directions)
        }
        gameObject.SetActive(false);
        Invoke("ReloadLevel", 3f);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void InitializePlayer()
    {
        this.GetComponent<CapsuleCollider2D>().isTrigger = false;
        this.animator.SetTrigger("Spawn");
        isSpawning = false;
        isInvulnerable = false;
        rigidbody2D.gravityScale = defaultGravityScale;
        GameObject.Find("Main Camera").GetComponent<CameraScript>().SetPlayerTransform(base.transform);
        rigidbody2D.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        soundManager.PlaySound(ESource.Megaman, spawnSound);
    }

    public void DoDamage()
    {
        if (!isInvulnerable)
        {
            animator.SetBool("isHurt", true);
            this.isInvulnerable = true;
            this.health--;
            if (health <= 0)
            {
                KillPlayer(true);
            }
            else
            {
                StartCoroutine(this.DamageEffect());
                SoundManager.instance.PlaySound(ESource.Megaman, hurtSound);
            }
        }
    }

    private IEnumerator DamageEffect()
    {
        float xDirection = isFacingRight ? -1f : 1f;
        inputEnable = false;

        if (!isGrounded)
        {
            rigidbody2D.AddForce(new Vector2(xDirection, 13f), ForceMode2D.Impulse);
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(xDirection, 5f), ForceMode2D.Impulse);
        }

        for (float i = 0f; i < 0.3; i += 0.1f)
        {
            hurtEffectSprite.enabled = false;
            this.spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            this.spriteRenderer.enabled = true;
            hurtEffectSprite.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        while (!isGrounded)
        {
            hurtEffectSprite.enabled = false;
            this.spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            this.spriteRenderer.enabled = true;
            hurtEffectSprite.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        inputEnable = true;
        animator.SetBool("isHurt", false);
        hurtEffectSprite.enabled = false;
        for (float i = 0f; i < 1; i += 0.1f)
        {
            this.spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            this.spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        this.isInvulnerable = false;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
}
