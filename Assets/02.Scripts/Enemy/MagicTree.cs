using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTree : Monster
{
    protected override void Awake()
    {
        maxHp = 45;
        walkSpeed = 2f;
        traceSpeed = 5f;
        traceDis = 15f;
        attackDis = 3.5f;
        traceStopDist = 3.5f;
        base.Awake();
    }
}
