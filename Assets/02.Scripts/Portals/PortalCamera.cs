using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    Transform mainCamera;
    public Transform thisPortal;
    public Transform connectPortal;
    public Vector3 offset;
    public float angluar;

    private void Awake()
    {
        mainCamera = Camera.main.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 offsetFromPortal = mainCamera.position - connectPortal.position;
        transform.position = thisPortal.position + offsetFromPortal + offset;

        float angularDifferencePortalRot = Quaternion.Angle(thisPortal.rotation, connectPortal.rotation)/angluar;

        Quaternion portalRotDifference = Quaternion.AngleAxis(angularDifferencePortalRot, Vector3.up);
        Vector3 dir = portalRotDifference * mainCamera.forward;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
