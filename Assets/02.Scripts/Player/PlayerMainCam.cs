using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainCam : MonoBehaviour
{
    [SerializeField]
    private float offsetX = 0.76f;
    [SerializeField]
    private float offsetY = 9.5f;
    [SerializeField]
    private float offsetZ = -7.2f;

    public GameObject player;

    [SerializeField]
    private float cameraSpeed = 4f;

    Vector3 forward, right;


    private void Start()
    {
        init();
    }
    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);
    }

    private void init()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    private void move()
    {
        Vector3 rightMove = right * cameraSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMove = forward * cameraSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 finalMove = forwardMove + rightMove;
        Vector3 dir = Vector3.Normalize(finalMove);

        if (dir != Vector3.zero)
        {
            transform.forward = dir;
            transform.position += finalMove;
        }
    }
}