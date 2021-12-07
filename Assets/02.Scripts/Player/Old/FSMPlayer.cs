using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMPlayer : playerFSMBase //playerFSMBase 스크립트로부터 상속 
{
    public int currHP = 120;
    public int maxHP = 120;
    //public int exp = 0;
    //public int level = 1;
    public int gold = 0;
    public float moveSpeed = 10.0f;
    public float turnSpeed = 360.0f;
    public Transform movePoint;

    int layerMask;

    public string clickLayer = "Click";
    public string blockLayer = "Block";
    public string enemyLayer = "Enemy";


    void Awake()
    {
        movePoint = GameObject.FindGameObjectWithTag("movePoint").transform;
        movePoint.gameObject.SetActive(false);
        layerMask = LayerMask.GetMask(clickLayer, blockLayer, enemyLayer);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100f, layerMask))
            {
                int layer = hitInfo.transform.gameObject.layer;

                if (layer == LayerMask.NameToLayer(clickLayer))
                {
                    Vector3 dest = hitInfo.point;
                    movePoint.transform.gameObject.SetActive(true);
                }
            }
        }
    }


    protected override IEnumerator Idle()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator Run()
    {
        do
        {
            yield return null;

            if (playerMoveUtil.MoveFrame(controller, movePoint, moveSpeed, turnSpeed) == 0)
            {
                movePoint.gameObject.SetActive(false);
                setState(playerState.Idle);
                break;
            }
        }

        while (!isNewState);
    }

    protected virtual IEnumerator Jump()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator Die()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator Damage()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator normalAttack()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator chopAttack()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator sliceAttack()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator pickAttack()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator Stun()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }

    protected virtual IEnumerator Heal()
    {
        do
        {
            yield return null;
        }
        while (!isNewState);
    }
}




