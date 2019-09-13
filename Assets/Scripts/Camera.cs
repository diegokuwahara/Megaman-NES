using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float posicaoMinimaX;
    public float posicaoMinimaY;

    private Vector2 velocity;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = player.position.x < posicaoMinimaX ? base.transform.position.x : player.position.x;
        float posY = player.position.y < posicaoMinimaY ? base.transform.position.y : player.position.y;
                     
        base.transform.position = new Vector3(posX, posY, base.transform.position.z);
    }
}
