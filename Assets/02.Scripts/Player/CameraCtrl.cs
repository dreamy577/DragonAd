using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; //카메라의 추적 대상 = 플레이어 
    public Vector3 offset = new Vector3(0, 1.0f, -1.0f); //카메라 이동 범위 보정값 

    public float currentZoom = 7.0f; //줌 기본값 

    float minZoom = 5.0f; //최소 줌 
    float maxZoom = 12.0f; //최대 줌 


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) //카메라 좌측 이동 
        {
            transform.RotateAround(target.position, Vector3.up, 5.0f);

            offset = transform.position - target.position;
            offset.Normalize();

        }

        if(Input.GetKeyDown(KeyCode.Alpha5)) //카메라 우측 이동 
        {
            transform.RotateAround(target.position, Vector3.up, -5.0f);

            offset = transform.position - target.position;
            offset.Normalize();
        }

        currentZoom -= Input.GetAxis("Mouse ScrollWheel");
        //마우스 휠로 줌 인/아웃 

        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        //줌 최소/최대 설정 
    }

    private void LateUpdate() //변경된 카메라 위치 적용 
    {
        transform.position = target.position + offset * currentZoom;
        //타겟으로부터 오프셋만큼 떨어져있도록 설정 

        transform.LookAt(target);
        //타겟을 향하도록 
    }
}
