using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRenderer : MonoBehaviour
{
    Renderer obsRenderer;
    Transform player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        float dis = Vector3.Distance(transform.position, player.position);
        Vector3 dir = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, dis))
        {
            obsRenderer = hit.collider.gameObject.GetComponentInChildren<Renderer>();
            if (obsRenderer != null)
            {
                Material mat = obsRenderer.material;
                Color matColor = mat.color;
            }
        }
    }
}
