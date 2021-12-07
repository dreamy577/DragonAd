using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    Transform player;
    public Transform connectPos;
    public Transform teleportPos;
    public GameObject renderObj;
    public float angle = 180;

    bool isSetting;
    bool isOverlapping = false;
    public bool isTeleported = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (renderObj != null)
        {
            if (isOverView(gameObject) && !isTeleported)
                renderObj.SetActive(false);
            else
                renderObj.SetActive(true);
        }


        if (isOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
            StartCoroutine(SetPlayerRootMotion());

            if (dotProduct < 0f)
            {
                float rotDiff = -Quaternion.Angle(transform.rotation, connectPos.rotation);
                rotDiff += angle;
                player.Rotate(Vector3.up, rotDiff);

                player.position = teleportPos.position;



                connectPos.GetComponent<Teleporter>().isTeleported = false;
                if (connectPos.GetComponent<Teleporter>().renderObj == null)
                    connectPos.gameObject.SetActive(false);
                isTeleported = true;
                isOverlapping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isOverlapping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOverlapping = false;
    }

    IEnumerator SetPlayerRootMotion()
    {
        isSetting = true;
        player.gameObject.GetComponentInChildren<Animator>().applyRootMotion = false;
        yield return new WaitForSeconds(0.1f);
        player.gameObject.GetComponentInChildren<Animator>().applyRootMotion = true;
        isSetting = false;
    }

    bool isOverView(GameObject target)
    {
        Vector3 ScreenPoint = Camera.main.WorldToViewportPoint(target.transform.position);

        if (Vector3.Distance(transform.position, Camera.main.transform.position) > 60 ||
            ScreenPoint.x < 0 || ScreenPoint.y < 0 || ScreenPoint.z < 0 ||
            ScreenPoint.x > Camera.main.pixelWidth || ScreenPoint.y > Camera.main.pixelHeight)
            return true;
        else
            return false;
    }
}
