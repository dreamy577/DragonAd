using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Alive : MonoBehaviour
{
    Rigidbody rb;
    public float curHp;  //현재HP
    protected float maxHp=-1;  //최대HP (-1은 에러 체크용)
    protected float atk=-1;    //공격력    (-1은 에러 체크용)
    protected bool isDie;   //해당 생물?(플레이어든 몬스터든)의 사망 여부 판단 변수

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        curHp = maxHp;
        isDie = false;
    }

    /// <summary>
    /// damage만큼 현재 채력에서 감산 및 사망여부 체크
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void Damaged(float damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            curHp = 0;
            isDie = true;
        }
    }

    protected virtual void Die()
    {
        print("Die Error!");
    }

}
