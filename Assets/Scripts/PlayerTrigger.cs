using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private BoxCollider2D ladderTop;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void ScriptThatTurnsPlatformBackOn()
    {
        ladderTop.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LadderTop") && Input.GetAxisRaw("Vertical") < 0)
        {
            ladderTop = collision.gameObject.GetComponent<BoxCollider2D>();
            ladderTop.enabled = false;
            Invoke("ScriptThatTurnsPlatformBackOn", 0.5f);
        } else if (collision.CompareTag("Enemy"))
        {
            player.DoDamage();
        }
    }
}
