using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; //ī�޶��� ���� ��� = �÷��̾� 
    public Vector3 offset = new Vector3(0, 1.0f, -1.0f); //ī�޶� �̵� ���� ������ 

    public float currentZoom = 7.0f; //�� �⺻�� 

    float minZoom = 5.0f; //�ּ� �� 
    float maxZoom = 12.0f; //�ִ� �� 


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) //ī�޶� ���� �̵� 
        {
            transform.RotateAround(target.position, Vector3.up, 5.0f);

            offset = transform.position - target.position;
            offset.Normalize();

        }

        if(Input.GetKeyDown(KeyCode.Alpha5)) //ī�޶� ���� �̵� 
        {
            transform.RotateAround(target.position, Vector3.up, -5.0f);

            offset = transform.position - target.position;
            offset.Normalize();
        }

        currentZoom -= Input.GetAxis("Mouse ScrollWheel");
        //���콺 �ٷ� �� ��/�ƿ� 

        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        //�� �ּ�/�ִ� ���� 
    }

    private void LateUpdate() //����� ī�޶� ��ġ ���� 
    {
        transform.position = target.position + offset * currentZoom;
        //Ÿ�����κ��� �����¸�ŭ �������ֵ��� ���� 

        transform.LookAt(target);
        //Ÿ���� ���ϵ��� 
    }
}
