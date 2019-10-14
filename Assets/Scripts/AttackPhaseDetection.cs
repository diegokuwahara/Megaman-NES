using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPhaseDetection : MonoBehaviour
{
    private Vector3 playerPosition;
    private Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Logica de ataque
        }
    }
}
