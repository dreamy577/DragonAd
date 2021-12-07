using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss_Active : MonoBehaviour
{
    public Transform sceneLookAt;
    public GameObject activeEffect;
    public GameObject Boss;
    public CinemachineVirtualCamera playerCmVcam;
    public CinemachineVirtualCamera ScenceCam;

    CinemachineTransposer transposer;
    private void Awake()
    {
        transposer = playerCmVcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(ActiveBoss());

    }
    IEnumerator ActiveBoss()
    {
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        activeEffect.SetActive(true);
        ScenceCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        Boss.SetActive(true);
        yield return new WaitForSeconds(2.8f);
        ScenceCam.gameObject.SetActive(false);
    }

}
