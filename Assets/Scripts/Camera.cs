﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float posicaoMinimaX;
    public float posicaoMinimaY;

    private Vector2 velocity;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        float posX = base.transform.position.x;
        float posY = base.transform.position.y;
        
        if (player != null)
        {
            posX = player.position.x > posicaoMinimaX ? posX : player.position.x;
            posY = player.position.y > posicaoMinimaY ? posY : player.position.y;
        }


        base.transform.position = new Vector3(posX, posY, base.transform.position.z);
    }
}
