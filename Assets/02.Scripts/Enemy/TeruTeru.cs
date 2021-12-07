using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeruTeru : Monster
{
    protected override void Awake()
    {
        maxHp = 40;
        walkSpeed = 3f;
        traceSpeed = 5f;
        traceDis = 18f;
        attackDis = 5f;
        traceStopDist = 1.5f;
        base.Awake();
    }



}
