using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDirection = ((Vector3.forward * v) + (Vector3.right * h)).normalized;
        transform.Translate(moveDirection * 10 * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up * 5 * Time.deltaTime * r);
    }
}
