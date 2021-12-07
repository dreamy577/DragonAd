using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(NavMeshAgent))]
public class Monster : Alive
{
    #region 변수 및 프로퍼티

    public enum State
    {
        IDLE,
        MOVE,
        TRACE,
        ATTACK,
        DAMAGED,
        DIE,
    }

    public State state = State.IDLE;

    #region NavMesh 관련

    [Header("NavMesh 관련")]
    public Transform spawnPoint;
    public GameObject wayPoint;
    public BoxCollider wayPointArea;

    protected NavMeshAgent agent;

    protected float dist;
    protected float traceStopDist;

    protected bool isSetWayPoint = true;
    protected bool isArrive = false;

    #endregion

    #region 아이템 드롭 관련

    public GameObject[] dropItem;

    #endregion

    #region UI 관련

    [Header("UI 관련")]
    public CinemachineOrbitalTransposer vCam;
    public Transform damageTextPos;
    public Vector3 damageTextOffset;

    public Transform standardPos;
    protected Image hpBar;
    protected Image hpFill;
    public Vector3 hpBarOffset;

    #endregion

    #region 데미지 관련

    protected GameObject hitEffectPref;
    protected ParticleSystem hitEffect;
    protected bool isDamaged = false;
    public bool isCanDamage = true;

    protected Coroutine idleOrAttack;
    protected Coroutine idleOrMove;

    #endregion

    #region 그 외 스탯에 따른 기타 요소 관련

    protected ParticleSystem puff;
    protected Transform playerTr;
    protected Animator animator;
    protected EnemyManager enemyManager;
    protected Collider[] _collider;

    protected bool isStartDie = false;
    protected bool startIdleOrMove = false;
    protected bool startIdleOrAttack = false;

    #endregion

    #region 애니메이터 파라미터 변수

    readonly protected int hashIdle = Animator.StringToHash("IDLE");
    readonly protected int hashMove = Animator.StringToHash("MOVE");
    readonly protected int hashAttack = Animator.StringToHash("ATTACK");
    readonly protected int hashDamaged = Animator.StringToHash("DAMAGED");
    readonly protected int hashDie = Animator.StringToHash("DIE");
    readonly protected int hashSkill = Animator.StringToHash("SKILL");


    #endregion

    #region 프로퍼티

    bool patrolling;
    public bool isPatrolling
    {
        get { return patrolling; }
        protected set
        {
            patrolling = value;

            if (patrolling)
                agent.speed = walkSpeed;

            WalkTarget();
        }
    }

    Vector3 _target;
    /// <summary>
    /// TRACE 시 추적 대상
    /// </summary>
    public Vector3 target
    {
        get { return _target; }
        protected set
        {
            _target = value;
            agent.speed = traceSpeed;
            TraceTarget(target);
        }
    }

    /// <summary>
    /// 추적 거리
    /// </summary>

    public float traceDis
    {
        get;
        protected set;
    } = -4;

    /// <summary>
    /// 공격 가능 거리
    /// </summary>

    public float attackDis
    {
        get;
        protected set;
    } = -4;

    /// <summary>
    /// 일반 이동 속도
    /// </summary>
    public float walkSpeed
    {
        get;
        protected set;
    } = -4;

    /// <summary>
    /// 추적 시 이동 속도
    /// </summary>
    public float traceSpeed
    {
        get;
        protected set;
    } = -4;

    #endregion 프로퍼티 영역



    #endregion 변수 및 프로퍼티 영역

    /*==========================================================================================*/

    #region 유니티 함수
    protected override void Awake()
    {
        base.Awake();

        vCam = GameObject.Find("CM vcam0 (2)").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent< CinemachineOrbitalTransposer>();

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _collider = GetComponentsInChildren<Collider>();

        hpBar = GetComponentInChildren<Image>();
        hpFill = hpBar.transform.GetChild(0).GetComponent<Image>();

        puff = transform.Find("SmokePuff").GetComponent<ParticleSystem>();

        hitEffectPref = Resources.Load<GameObject>("hitEffect");
        GameObject hitVfx = Instantiate(hitEffectPref, transform.parent);
        hitEffect = hitVfx.GetComponent<ParticleSystem>();

        if (standardPos == null)
        {
            standardPos = gameObject.transform;
        }

        state = State.MOVE;

        curHp = maxHp;
        isDie = false;
    }

    protected override void OnEnable()
    {
        puff.Play();
        base.OnEnable();
        state = State.MOVE;

        transform.position = transform.parent.position;

        for (int a = 0; a < _collider.Length; a++)
            _collider[a].enabled = true;

        hpBar.gameObject.SetActive(false);

        isCanDamage = true;
        isDamaged = false;
        isArrive = false;
        isStartDie = false;
        isSetWayPoint = false;
        startIdleOrAttack = false;
        startIdleOrMove = false;

        SetWayPointPos();
        WalkTarget();
        StartCoroutine(StateCheck());
        StartCoroutine(Action());
    }

    protected virtual void Update()
    {
        if (isDie)
            Die();
        else
            CheckArrive();

    }

    protected void LateUpdate()
    {
        if (state == State.ATTACK)
        {
            transform.LookAt(new Vector3(playerTr.position.x, transform.position.y, playerTr.position.z));
        }

        SetDamageUIs();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon") && isCanDamage)
        {
            isDamaged = true;
            isCanDamage = false;
            hitEffect.transform.position = collision.contacts[0].point + Vector3.up;
            hitEffect.Play();
            Damaged(5);            // ← 여기에 데미지 함수 넣기
        }
    }

    #endregion

    /*==========================================================================================*/

    #region 작성한 함수

    /// <summary>
    /// 추적 대상 설정 함수(프로퍼티 용)
    /// </summary>
    /// <param name="target"></param>
    protected virtual void TraceTarget(Vector3 target)
    {
        if (agent.isPathStale)
            return;

        agent.destination = target;
        agent.isStopped = false;
    }

    protected virtual void Stop()
    {
        agent.speed = 0;
        agent.isStopped = true;
        agent.ResetPath();
    }

    /// <summary>
    /// 해당 몬스터 이동 구역 내의 랜덤 좌표를 wayPoint로 설정
    /// </summary>
    /// <returns></returns>
    protected void SetWayPointPos()
    {
        isSetWayPoint = true;
        isArrive = false;
        Vector3 basePos = wayPointArea.transform.position;
        Vector3 size = wayPointArea.size;

        float posX = basePos.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posZ = basePos.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 pos = new Vector3(posX, 8f, posZ);

        wayPoint.transform.position = pos;
    }

    /// <summary>
    /// WayPoint 설정
    /// </summary>
    protected void WalkTarget()
    {
        if (agent.isPathStale)
            return;

        agent.destination = wayPoint.transform.position;
        agent.isStopped = false;
    }

    /// <summary>
    /// 목표 좌표에 도달했는지 체크
    /// </summary>
    protected void CheckArrive()
    {
        if (!isPatrolling)
            return;

        else if (agent.remainingDistance <= 0.5f)
            isArrive = true;
    }

    /// <summary>
    /// 플레이어가 추적 범위 내에 없을 시 IDLE 혹은 순찰하도록 하는 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IdleOrMove()
    {
        startIdleOrMove = true;
        startIdleOrAttack = false;
        while (dist > traceDis && !isDamaged)
        {
            yield return new WaitForSeconds(0.01f);
            if (state == State.TRACE || state == State.ATTACK || state == State.DAMAGED)
            {
                state = State.IDLE;
                yield return new WaitForSeconds(Random.Range(1f, 2.5f));
            }
            else if (state == State.MOVE)
            {
                if (isArrive)
                    state = State.IDLE;
                else
                    isPatrolling = true;
            }
            else if (state == State.IDLE)
            {
                if (isArrive)
                {
                    yield return new WaitForSeconds(Random.Range(4, 9));
                    isPatrolling = false;
                    isSetWayPoint = false;
                    if (!isSetWayPoint)
                    {
                        SetWayPointPos();
                        WalkTarget();
                    }
                }
                else
                {
                    state = State.MOVE;
                }
            }
        }
        startIdleOrMove = false;
        StopCoroutine(IdleOrMove());
    }

    string name_;
    /// <summary>
    /// 플레이어가 공격 범위 내에 있을 때 공격과 IDLE(딜레이) 상태를 반복하도록 하는 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IdleOrAttack()
    {
        startIdleOrMove = false;
        startIdleOrAttack = true;
        while (dist <= attackDis && !isDamaged)
        {
            yield return null;
            if (dist <= attackDis)
            {
                if (state == State.IDLE)
                {
                    state = State.ATTACK;
                    yield return new WaitForSeconds(getAnimLength());
                }
                else if (state == State.ATTACK || state == State.DAMAGED)
                {
                    state = State.IDLE;
                    yield return new WaitForSeconds(Random.Range(0.7f, 1f));
                }
                else
                {
                    state = State.ATTACK;
                    yield return new WaitForSeconds(getAnimLength());
                }
            }
            else
                break;
        }
        startIdleOrAttack = false;
        WalkTarget();
    }

    protected override void Damaged(float damage)
    {
        base.Damaged(damage);

        GameObject damageTxt = Instantiate(Resources.Load<GameObject>("damageText"), Camera.main.WorldToScreenPoint(damageTextPos.position), Quaternion.identity);
        damageTxt.GetComponent<FloatDamageText>().damage = damage;
        damageTxt.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void skillDamage(float damage)
    {
        isDamaged = true;
        StartCoroutine(skillCanDamage());
        Damaged(damage);
    }

    IEnumerator skillCanDamage()
    {
        isCanDamage = false;
        yield return new WaitForSeconds(0.1f);
        isCanDamage = true;
    }

    protected void SetDamageUIs()
    {
        damageTextPos.transform.position = transform.position + damageTextOffset;

        hpBar.transform.SetParent(GameObject.Find("Canvas").transform);
        hpBar.transform.position = Camera.main.WorldToScreenPoint((standardPos.position + hpBarOffset)/*-vCam.m_FollowOffset*/);
        hpFill.fillAmount = curHp / maxHp;
    }

    protected override void Die()
    {
        if (idleOrMove != null)
            StopCoroutine(idleOrMove);
        if (idleOrAttack != null)
            StopCoroutine(idleOrAttack);

        state = State.DIE;

        for (int a = 0; a < _collider.Length; a++)
            _collider[a].enabled = false;

        if (!isStartDie)
            StartCoroutine(DieDelay());
    }

    IEnumerator DieDelay()
    {
        isStartDie = true;
        yield return new WaitForSeconds(getAnimLength() + 2f);
        enemyManager.emptySpawnPoints.Add(spawnPoint);
        enemyManager.spawnPoints.Remove(spawnPoint);
        spawnPoint = null;
        DropItem();
        UIManaager.Instance.GetCoin();
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    protected void DropItem()
    {
        int random = Random.Range(0, 101);
        if (random < 100)
            Instantiate(dropItem[Random.Range(0,dropItem.Length)], transform.position + (Vector3.up * 4), Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

    
    /// <summary>
    /// 각 애니메이션 클립의 재생 길이 반환
    /// </summary>
    /// <returns></returns>
    protected virtual float getAnimLength()
    {
        string name = string.Empty;

        switch (state)
        {
            case State.IDLE:
                name = "Idle";
                break;

            case State.MOVE:
                name = "Move";
                break;

            case State.TRACE:
                name = "Move";
                break;

            case State.ATTACK:
                name = "Atk";
                break;

            case State.DAMAGED:
                name = "Damage";
                break;

            case State.DIE:
                name = "Die";
                break;


            default:
                return 0;

        }

        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int a = 0; a < ac.animationClips.Length; a++)
        {
            if (ac.animationClips[a].name == name)
            {
                time = ac.animationClips[a].length / animator.GetCurrentAnimatorStateInfo(0).speed;
                name_ = ac.animationClips[a].name;
            }
        }

        return time;

        //return animator.GetCurrentAnimatorStateInfo(0).length/animator.GetCurrentAnimatorStateInfo(0).speed;
    }

    protected IEnumerator StateCheck()
    {
        while (!isDie)
        {
            if (state == State.DIE)
                yield break;

            else
            {
                if (isDamaged)
                {
                    state = State.DAMAGED;
                    yield return new WaitForSeconds(getAnimLength());
                    isDamaged = false;
                    isCanDamage = true;
                }
                else
                {
                    dist = Vector3.Distance(playerTr.position, transform.position);

                    if (dist <= attackDis && !startIdleOrAttack)
                    {
                        hpBar.gameObject.SetActive(true);
                        idleOrAttack = StartCoroutine(IdleOrAttack());
                    }

                    else if (dist <= traceDis && !startIdleOrAttack)
                    {
                        state = State.TRACE;
                        hpBar.gameObject.SetActive(true);
                        startIdleOrMove = false;
                        startIdleOrAttack = false;
                    }

                    else if (dist > traceDis && !startIdleOrMove)
                    {
                        hpBar.gameObject.SetActive(false);
                        idleOrMove = StartCoroutine(IdleOrMove());
                    }

                }
            }

            yield return new WaitForSeconds(0.1f);

        }

    }

    protected virtual IEnumerator Action()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);

            switch (state)
            {
                case State.IDLE:
                    Stop();
                    animator.SetBool(hashAttack, false);
                    animator.SetBool(hashIdle, true);
                    animator.SetBool(hashMove, false);
                    break;

                case State.MOVE:
                    isPatrolling = true;
                    agent.stoppingDistance = 0.2f;
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    target = playerTr.position;
                    agent.stoppingDistance = traceStopDist;
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    Stop();
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetTrigger(hashAttack);
                    break;

                case State.DAMAGED:
                    Stop();
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetTrigger(hashDamaged);
                    break;

                case State.DIE:
                    Stop();
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetTrigger(hashDie);
                    break;
            }

        }
    }

    #endregion
}

