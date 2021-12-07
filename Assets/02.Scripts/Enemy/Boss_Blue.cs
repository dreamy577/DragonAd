using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Blue : Boss
{
    protected override void Awake()
    {
        base.Awake();
        maxHp = 500;
        atk = 99;
        attackDis = 10;
        skillDis = 20;
        traceSpeed = 3;
    }

    /// <summary>
    /// 1페이즈 패턴 코루틴
    /// </summary>
    /// <returns></returns>
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
                isStun = false;
                isCanStun = true;
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

                    int tmp = Random.Range(0, 1001);

                    if (tmp < 500)
                        state = State.ATTACK1;
                    else
                        state = State.ATTACK2;

                    yield return new WaitForSeconds(getAnimLength());
                    state = State.IDLE;
                }
                else if (!fov.isViewPlayer() || dist > attackDis)
                {
                    if (state == State.IDLE)
                        yield return new WaitForSeconds(Random.Range(0.6f, 1f));

                    state = State.MOVE;
                }
                else
                    state = State.IDLE;
            }


        }
    }

    /// <summary>
    /// 2페이즈 패턴 코루틴
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Phase2()
    {
        isPhase2 = true;
        state = State.PHASE2;
        yield return new WaitForSeconds(getAnimLength());
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
                yield return new WaitForSeconds(Time.deltaTime / 10);
                state = State.STUN;
                yield return new WaitForSeconds(getAnimLength() + 3f);
                state = State.IDLE;
                isStun = false;
                isCanStun = true;
            }
            else if(!isStun)
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

                    int tmp = Random.Range(0, 1501);
                    if (tmp < 500)
                        state = State.ATTACK1;
                    else if (tmp < 1000)
                        state = State.ATTACK2;
                    else
                    {
                        state = State.DEFENCE;
                        isCanDamage = false;
                        isCanStun = false;
                    }

                    yield return new WaitForSeconds(getAnimLength());
                    isCanDamage = true;
                    isCanStun = true;
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

                    int tmp = Random.Range(0, 1001);

                    if (tmp < 700)  //70% 확률로 스킬 사용
                    {
                        state = State.SKILL;
                        yield return new WaitForSeconds(getAnimLength());
                        state = State.IDLE;
                    }
                    else //30% 확률로 일반 공격을 위해 플레이어 추적
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
