using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Red : Boss
{

    protected override void Awake()
    {
        base.Awake();
        maxHp = 500;
        atk = 99;
        attackDis = 15;
        skillDis = 25;
        traceSpeed = 4;
    }

    protected override IEnumerator Phase1()
    {
        state = State.SCREAM;
        yield return new WaitForSeconds(getAnimLength());
        state = State.IDLE;
        yield return new WaitForSeconds(getAnimLength());

        while (!isDie)
        {
            yield return new WaitForSeconds(0.01f);

            if (state == State.DIE)
                yield break;

            dist = Vector3.Distance(playerTr.position, transform.position);

            if (isStun)
            {
                isCanStun = false;
                state = State.STUN_START;
                yield return new WaitForSeconds(getAnimLength());
                state = State.STUN;
                yield return new WaitForSeconds(getAnimLength() + 3f);
                state = State.IDLE;
                isCanStun = true;
                isStun = false;
            }
            else if (!isStun)
            {
                if (dist < skillDis && fov.isViewPlayer())
                {
                    if (state == State.IDLE)
                        yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
                    else if (state == State.MOVE)
                    {
                        state = State.IDLE;
                        yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
                    }

                    int tmp = Random.Range(0, 101);

                    if (tmp < 50)
                    {
                        while (dist > attackDis || !fov.isViewPlayer())
                        {
                            yield return new WaitForSeconds(0.01f);
                            dist = Vector3.Distance(playerTr.position, transform.position);
                            state = State.MOVE;
                        }
                        state = State.ATTACK1;
                    }
                    else
                        state = State.ATTACK2;

                    yield return new WaitForSeconds(getAnimLength());
                    state = State.IDLE;
                }
                else if (dist > skillDis || !fov.isViewPlayer())
                {
                    if (state == State.IDLE)
                        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));

                    state = State.MOVE;
                }
                else
                    state = State.IDLE;

            }
        }
    }

    protected override IEnumerator Phase2()
    {
        isPhase2 = true;
        isStun = false;
        isCanStun = false;
        isCanDamage = false;

        state = State.PHASE2;
        yield return new WaitForSeconds(getAnimLength());
        state = State.SCREAM;
        yield return new WaitForSeconds(getAnimLength());
        state = State.IDLE;
        yield return new WaitForSeconds(getAnimLength());

        isCanStun = true;
        isCanDamage = true;

        while (!isDie)
        {
            yield return new WaitForSeconds(0.01f);

            if (state == State.DIE)
                yield break;

            dist = Vector3.Distance(playerTr.position, transform.position);

            if (isStun)
            {
                isCanStun = false;
                state = State.STUN_START;
                yield return new WaitForSeconds(getAnimLength());
                state = State.STUN;
                yield return new WaitForSeconds(getAnimLength() + 3f);
                state = State.IDLE;
                isCanStun = true;
                isStun = false;
            }
            else
            {
                if (dist < attackDis && fov.isViewPlayer())
                {
                    if (state == State.IDLE)
                        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));
                    else if (state == State.MOVE)
                    {
                        state = State.IDLE;
                        yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
                    }

                    state = State.ATTACK1;
                    yield return new WaitForSeconds(getAnimLength());
                    state = State.IDLE;
                }
                else if (dist < skillDis && fov.isViewPlayer())
                {
                    if (state == State.IDLE)
                        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));
                    else if (state == State.MOVE && dist < skillDis)
                    {
                        state = State.IDLE;
                        yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
                    }

                    int tmp = Random.Range(0, 1781);

                    if (tmp < 540)  //30% 확률로 뛰어들기(일반 공격)
                    {
                        state = State.ATTACK2;
                        yield return new WaitForSeconds(getAnimLength());
                        state = State.IDLE;
                    }
                    else if (tmp < 810) //45% 확률로 스킬 사용
                    {
                        state = State.SKILL;
                        yield return new WaitForSeconds(getAnimLength());
                        state = State.IDLE;
                    }
                    else //20% 확률로 일반 공격을 위해 플레이어 추적
                    {
                        while (!fov.isViewPlayer() || dist > attackDis)
                        {
                            dist = Vector3.Distance(playerTr.position, transform.position);
                            state = State.MOVE;
                            yield return new WaitForSeconds(0.01f);
                        }
                    }
                }
                else if (!fov.isViewPlayer() || dist > skillDis)
                {
                    if (state == State.IDLE)
                        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));

                    state = State.MOVE;
                }
                else
                    state = State.IDLE;
            }
        }
    }
}
