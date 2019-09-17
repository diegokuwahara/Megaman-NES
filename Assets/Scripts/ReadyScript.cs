using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScript : MonoBehaviour
{
    public Player playerPrefab;
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CriaPlayer", 2.5f);
        Destroy(gameObject, 2.5f);
    }

    void CriaPlayer()
    {
        Instantiate(playerPrefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 10), playerPrefab.transform.rotation);
    }

}
