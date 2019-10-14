using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blader : MonoBehaviour, IEnemy
{
    public int life;
    public float patrolTurnTime;
    private float currentTurnTime;
    private bool isFacingRight = false;
    private Rigidbody2D rigidbody2D;
    private int walkSpeed = 3;
    private bool isDead = false;

    #region [ Métodos públicos ]
    public void DoDamage()
    {
        life--;
        if (life <= 0)
        {
            this.KillEnemy();
        }
    }

    #endregion

    #region [ Métodos privados ]
    private void Start()
    {
        currentTurnTime = Time.time + patrolTurnTime;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDead)
        {
            rigidbody2D.velocity = -base.transform.right * walkSpeed;

            if (currentTurnTime <= Time.time)
            {
                this.Flip();
                currentTurnTime = Time.time + patrolTurnTime;
            }
        }
        
    }

    private void KillEnemy()
    {
        isDead = true;
        rigidbody2D.velocity = base.transform.right * 0;
        this.GetComponent<Animator>().SetTrigger("Die");
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(gameObject, 0.2f);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        base.transform.Rotate(0f, 180, 0f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            Destroy(gameObject);
        }
    }

    #endregion

}
