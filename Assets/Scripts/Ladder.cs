using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void ScriptThatTurnsPlatformBackOn()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundCheck") && Input.GetAxisRaw("Vertical") < 0)
        {
            Debug.Log("Entrou");
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Invoke("ScriptThatTurnsPlatformBackOn", 0.5f);
        }
    }
}
