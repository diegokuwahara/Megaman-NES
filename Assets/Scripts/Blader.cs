using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blader : MonoBehaviour, IEnemy
{
    public int life;

    public void DoDamage()
    {
        life--;
        if (life <= 0)
        {
            this.KillEnemy();
        }
    }

    public void KillEnemy()
    {
        this.GetComponent<Animator>().SetTrigger("Die");
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(gameObject, 0.2f);
    }
}
