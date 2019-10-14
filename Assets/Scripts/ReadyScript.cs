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
        Invoke("CriaPlayer", 3.0f);
        Destroy(gameObject, 3.0f);
    }

    void CriaPlayer()
    {   
        Camera camera = mainCamera.GetComponent<Camera>();
        float cameraHeight = camera.orthographicSize;
        Instantiate(playerPrefab, new Vector3(mainCamera.transform.position.x, cameraHeight, 10), playerPrefab.transform.rotation);
    }
}
