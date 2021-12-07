using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonTon : Monster
{
    protected override void Awake()
    {
        maxHp = 20;
        walkSpeed = 3f;
        traceSpeed = 4.5f;
        traceDis = 15f;
        attackDis = 2.5f;
        traceStopDist = 2.3f;
        base.Awake();
    }
}
