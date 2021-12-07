using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))] //자동으로 캐릭터 컨트롤러 추가 

public class playerFSMBase : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;

    public playerState playrtst;
    public bool isNewState; //개체 상태 체크 

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        playrtst = playerState.Idle;
        StartCoroutine(FSMMain());
    }


    IEnumerator FSMMain() 
    {
        //상태 변경시 CHState.ToString() 실행, 첫 실행은 Idle
        while (true)
        {
            isNewState = false;
            yield return
            StartCoroutine(playrtst.ToString());
            //개체 상태 변경시마다 메소드 실행 
        }
    }

    public void setState(playerState newState)
    {
        isNewState = true;
        playrtst = newState; 
        //개체가 가진 애니메이터 컴포넌트의 파라미터에게 상태 변화값 전달
        anim.SetInteger("State", (int)playrtst);
    }

    protected virtual IEnumerator Idle() //모든 state는 Idle로 연결
    {
        do //1프레임당 1회 체크 
        {
            yield return null;
        }

        while (!isNewState); //do 종료 조건 
    }
}
