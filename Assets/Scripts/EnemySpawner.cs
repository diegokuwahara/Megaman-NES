using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IEnemyGenerator
{
    public GameObject Enemy;
    private GameObject enemySpawned;
    // Start is called before the first frame update

    public void SpawnEnemy()
    {
        if (enemySpawned == null)
            enemySpawned = Instantiate(Enemy, this.transform.position, this.transform.rotation);
    }
}
