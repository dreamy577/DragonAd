using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))] //�ڵ����� ĳ���� ��Ʈ�ѷ� �߰� 

public class playerFSMBase : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;

    public playerState playrtst;
    public bool isNewState; //��ü ���� üũ 

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
        //���� ����� CHState.ToString() ����, ù ������ Idle
        while (true)
        {
            isNewState = false;
            yield return
            StartCoroutine(playrtst.ToString());
            //��ü ���� ����ø��� �޼ҵ� ���� 
        }
    }

    public void setState(playerState newState)
    {
        isNewState = true;
        playrtst = newState; 
        //��ü�� ���� �ִϸ����� ������Ʈ�� �Ķ���Ϳ��� ���� ��ȭ�� ����
        anim.SetInteger("State", (int)playrtst);
    }

    protected virtual IEnumerator Idle() //��� state�� Idle�� ����
    {
        do //1�����Ӵ� 1ȸ üũ 
        {
            yield return null;
        }

        while (!isNewState); //do ���� ���� 
    }
}
