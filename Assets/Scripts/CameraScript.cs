using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float posicaoMinimaX;
    public float posicaoMinimaY;

    private Vector2 velocity;
    private Transform player;

    // Update is called once per frame
    void Update()
    {
        float posX = base.transform.position.x;
        float posY = base.transform.position.y;
        
        if (player != null)
        {
            posX = player.position.x > posicaoMinimaX ? player.position.x : posX;
            posY = player.position.y > posicaoMinimaY ? player.position.y : posY;
        }


        base.transform.position = new Vector3(posX, posY, base.transform.position.z);
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemyGenerator generator = collision.gameObject.GetComponent<IEnemyGenerator>();
        if (generator != null)
        {
            generator.SpawnEnemy();
        }
    }
}
