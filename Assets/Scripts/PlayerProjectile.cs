using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class PlayerProjectile : MonoBehaviour
{
    public float speed;
    public AudioClip damageHit;

    private Rigidbody2D rigidbody2D;
    private SoundManager soundManager;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = base.transform.right * speed;
        soundManager = SoundManager.instance;
    }

    void Update()
    {
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IEnemy enemy = collider.gameObject.GetComponent<IEnemy>();
        if (enemy != null)
        {
            soundManager.PlaySound(ESource.Enemy, damageHit);
            enemy.DoDamage();
            Destroy(gameObject, 0.03f);
        }
    }
}
