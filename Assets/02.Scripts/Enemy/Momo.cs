using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momo : Monster
{
    protected override void Awake()
    {
        maxHp = 30;
        walkSpeed = 3f;
        traceSpeed = 6f;
        traceDis = 15f;
        attackDis = 4f;
        traceStopDist = 3f;
        base.Awake();
    }

}
