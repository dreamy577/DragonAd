using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Monster
{
    protected override void Awake()
    {
        maxHp = 20;
        walkSpeed = 2f;
        traceSpeed = 5f;
        traceDis = 20f;
        attackDis = 3f;
        traceStopDist = 3f;
        base.Awake();
    }
}
