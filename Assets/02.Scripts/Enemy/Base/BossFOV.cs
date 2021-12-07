using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFOV : MonoBehaviour
{
    public float viewRange = 15f; //적의 추적 사정 거리 범위

    [Range(0, 360)]
    public float viewAngle = 120f;  //시야각을 최소 0~ 최대 360으로 설정

    Transform bossTr;
    Transform playerTr;

    private void Start()
    {
        bossTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool isViewPlayer()
    {
        //bool isView = false;
        //RaycastHit hit;

        //Vector3 dir = (playerTr.position - bossTr.position).normalized;

        //if (Physics.Raycast(bossTr.position, bossTr.forward, out hit, viewRange))
        //{
        //    isView = (hit.collider.CompareTag("Player"));
        //}
        //return isView;

        bool isView = false;

        Vector3 dir = (playerTr.position - bossTr.position).normalized;

        if (Vector3.Angle(bossTr.forward, dir) < viewAngle * 0.5f) //적의 시야각에 들어왔는지 판단
            isView = true;

        return isView;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;   //로컬 좌표계 기준으로 설정하기 위해 적 캐릭터의 회전값 중 Y값을 더해준다는데 다시 읽어봐도 뭔 말인지 모르겠다
        Vector3 viewAngle = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

        return viewAngle;
    }

}
