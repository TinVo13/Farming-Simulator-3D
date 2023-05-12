using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerPos;
    public float offsetZ = 3f;
    public float smoothing = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // playerPos = FindObjectOfType<SimpleSampleCharacterControl>().transform;
        playerPos = FindObjectOfType<PlayerControllerDemo>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z - offsetZ);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
