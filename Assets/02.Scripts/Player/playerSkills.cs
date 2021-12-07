using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSkills : MonoBehaviour
{
    public Transform other; //몬스터 위치 
    public float closeDistance = 4.0f; //거리 차이


    void Update()
    {
        pickAtk();
    }

    public void pickAtk() //광역기_이펙트 발생 시 일정 거리 안의 적 모두 사망 
    {
        if (other)
        {
            Vector3 offset = other.position - transform.position;
            float sqrLen = offset.sqrMagnitude;

            if (sqrLen < closeDistance * closeDistance)
            {
                GameObject.Destroy(other);
            }
        }
    }

}
