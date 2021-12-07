using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

[RequireComponent(typeof(NavMeshAgent))]

public class Boss : Alive
{
    #region 변수 및 프로퍼티
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK1,
        ATTACK2,
        DEFENCE,
        SKILL,
        STUN_START,
        STUN,
        SCREAM,
        PHASE2,
        DIE,
    }

    public State state;

    #region 상태머신(?)관련
    protected Transform playerTr;

    protected NavMeshAgent agent;
    protected Animator animator;

    protected BossFOV fov;

    protected Coroutine phase1;
    protected Coroutine phase2;


    protected ParticleSystem skillBress;
    protected Transform skillParent;
    protected Vector3 skillPos;
    protected Vector3 skillRot;

    protected List<Collider> colliders = new List<Collider>();

    protected float dist;

    public bool debug_startPhase2;
    public bool isPhase2 = false;
    protected bool isStun;
    protected bool isCanStun = true;
    protected bool isDamaged = false;
    protected bool isCanDamage = true;

    #endregion

    #region 사망 이벤트 관련

    protected VideoPlayer videoPlayer;
    protected Vector3 defaultPos;
    protected Quaternion defaultRot;

    public GameObject dieSceneCam1;
    public CinemachineVirtualCamera dieSceneCam2;

    public GameObject dragonGem;
    public GameObject portal;

    protected bool isStartCutScene;

    #endregion

    #region 애니메이터 파라미터
    readonly protected int hashIdle = Animator.StringToHash("IDLE");
    readonly protected int hashMove = Animator.StringToHash("MOVE");
    readonly protected int hashAttack1 = Animator.StringToHash("ATTACK1");
    readonly protected int hashAttack2 = Animator.StringToHash("ATTACK2");
    readonly protected int hashStun = Animator.StringToHash("STUN");
    readonly protected int hashPhase2 = Animator.StringToHash("PHASE2");
    readonly protected int hashDefence = Animator.StringToHash("DEFENCE");
    readonly protected int hashSkill = Animator.StringToHash("SKILL");
    readonly protected int hashDie = Animator.StringToHash("DIE");
    #endregion

    #region 프로퍼티

    public float defDis
    {
        get;
        protected set;
    } = -4;

    /// <summary>
    /// 추적 거리
    /// </summary>
    public float traceDis
    {
        get;
        protected set;
    } = -4;

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
    /// 공격 가능 거리
    /// </summary>
    public float attackDis
    {
        get;
        protected set;
    } = -4;

    /// <summary>
    /// 스킬 가능 거리
    /// </summary>
    public float skillDis
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
    #endregion
    #endregion

    /*==========================================================================================*/

    #region 유니티 함수
    protected override void Awake()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation;

        fov = GetComponent<BossFOV>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        videoPlayer = GetComponent<VideoPlayer>();
        skillBress = GetComponentInChildren<ParticleSystem>();
        skillParent = skillBress.transform.parent.GetComponent<Transform>();
        skillPos = skillBress.transform.localPosition;
        skillRot = skillBress.transform.localRotation.eulerAngles;

        Collider[] tmp = GetComponentsInChildren<Collider>();
        for (int a = 0; a < tmp.Length; a++)
        {
            tmp[a].gameObject.tag = "Enemy";
            tmp[a].gameObject.AddComponent<BossCollider>();
            tmp[a].GetComponent<BossCollider>().parentObj = gameObject;
            tmp[a].GetComponent<BossCollider>().idx = a;
            colliders.Add(tmp[a]);
        }

        playerTr = GameObject.FindWithTag("Player").transform;
        state = State.IDLE;

        maxHp = -99;
        atk = -99;
        attackDis = -9;
        traceSpeed = -9;

        isCanDamage = true;
        isDamaged = false;

        curHp = maxHp;
        isDie = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        phase1 = StartCoroutine(Phase1());
        StartCoroutine(Action());
    }

    protected void Update()
    {
        if (curHp <= maxHp * 0.5f && !isPhase2)
        {
            StopCoroutine(phase1);
            phase2 = StartCoroutine(Phase2());
        }

        if (debug_startPhase2 && !isPhase2)
        {
            StopCoroutine(phase1);
            phase2 = StartCoroutine(Phase2());
        }

        if (isDie)
            Die();
    }
    #endregion

    /*==========================================================================================*/

    #region 작성 함수
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

    /// <summary>
    /// NavMesh 이동 정지용
    /// </summary>
    protected virtual void Stop()
    {
        agent.speed = 0;
        agent.isStopped = true;
        agent.ResetPath();
    }

    protected float getAnimLength()
    {
        string name = string.Empty;

        switch (state)
        {
            case State.IDLE:
                name = "Idle";
                break;

            case State.MOVE:
                name = "Walk";
                break;

            case State.STUN:
                name = "Stun";
                break;

            case State.STUN_START:
                name = "Stun";
                break;

            case State.SKILL:
                name = "Skill";
                break;

            case State.DIE:
                name = "Scream";
                break;

            case State.ATTACK1:
                name = "Attack01";
                break;

            case State.ATTACK2:
                name = "Attack02";
                break;

            case State.SCREAM:
                name = "Scream";
                break;

            case State.DEFENCE:
                name = "Defend";
                break;

            default:
                return 0;

        }

        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int a = 0; a < ac.animationClips.Length; a++)
        {
            if (ac.animationClips[a].name == name)
                time = ac.animationClips[a].length;
        }

        return time;
    }

    /// <summary>
    /// 데미지 함수(피격받은 콜라이더의 인덱스 번호, 데미지량)
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="damage"></param>
    public void Damage(int idx, float damage)
    {
        base.Damaged(damage);
        if (!isDie)
            StartCoroutine(SetCollider_(idx));
    }

    public void Stun(int idx, float damage)
    {
        if (isCanStun)
        {
            Damage(idx, damage);
            isStun = true;
        }
    }

    /// <summary>
    /// 플레이어와 충돌 시 플레이어의 다중 피격 방지를 위한 콜라이더 제어
    /// </summary>
    /// <param name="idx"></param>
    public void ColliderSet(int idx)
    {
        StartCoroutine(SetCollider_(idx));
    }

    /// <summary>
    /// 충돌한 콜라이더 이외의 나머지 콜라이더들을 잠시 꺼주는 코루틴(과도한 다중 피격 방지)
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    protected IEnumerator SetCollider_(int idx)
    {
        if (isCanDamage)
        {
            for (int a = 0; a < colliders.Count; a++)
            {
                if (a != idx)
                {
                    colliders[a].enabled = false;
                }
            }

            yield return new WaitForSeconds(1f);

            foreach (Collider collider in colliders)
            {
                collider.enabled = true;
            }
        }
    }

    /// <summary>
    /// 1페이즈 패턴 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Phase1()
    {
        print("Now:: Phase 1");
        return null;
    }

    /// <summary>
    /// 2페이즈 패턴 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Phase2()
    {
        print("Now:: Phase 2");
        return null;
    }

    protected IEnumerator Action()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.01f);

            switch (state)
            {
                case State.SCREAM:
                    Stop();
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    animator.SetBool(hashDie, false);
                    break;

                case State.IDLE:
                    Stop();
                    animator.SetBool(hashIdle, true);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.MOVE:
                    target = playerTr.position;
                    animator.SetBool(hashMove, true);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.ATTACK1:
                    Stop();
                    animator.SetBool(hashAttack1, true);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.ATTACK2:
                    Stop();
                    animator.SetBool(hashAttack2, true);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.STUN_START:
                    Stop();
                    animator.SetTrigger(hashStun);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    state = State.STUN;
                    break;

                case State.STUN:
                    Stop();
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.PHASE2:
                    Stop();
                    animator.SetTrigger(hashPhase2);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.DEFENCE:
                    Stop();
                    animator.SetBool(hashDefence, true);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashSkill, false);
                    break;

                case State.SKILL:
                    Stop();
                    animator.SetBool(hashSkill, true);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    break;

                case State.DIE:
                    Stop();
                    animator.SetTrigger(hashDie);
                    animator.SetBool(hashIdle, false);
                    animator.SetBool(hashMove, false);
                    animator.SetBool(hashAttack1, false);
                    animator.SetBool(hashAttack2, false);
                    animator.SetBool(hashDefence, false);
                    animator.SetBool(hashSkill, false);
                    break;

            }
        }
    }

    protected override void Die()
    {
        state = State.DIE;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Collider collider in colliders)
        {
            collider.tag = "Untagged";
            Destroy(collider.GetComponent<BossCollider>());
            Destroy(collider.GetComponent<Rigidbody>());
        }

        if (!isStartCutScene)
            StartCoroutine(DieCutScene_());
    }

    protected IEnumerator DieCutScene()
    {
        isStartCutScene = true;
        yield return new WaitForSeconds(getAnimLength());
        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.length);
        videoPlayer.Stop();
        transform.position = defaultPos;
        transform.rotation = defaultRot;
        dragonGem.SetActive(true);
        portal.SetActive(true);
    }

    protected IEnumerator DieCutScene_()
    {
        isStartCutScene = true;
        yield return new WaitForSeconds(getAnimLength() - 1f);
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        transform.position = defaultPos;
        transform.rotation = defaultRot;
        dieSceneCam1.SetActive(true);
        yield return new WaitForSeconds(4f);
        dragonGem.SetActive(true);
        dieSceneCam2.gameObject.SetActive(true);
        while (dragonGem.transform.localPosition.y < 5)
        {
            yield return null;
            dieSceneCam2.m_Lens.FieldOfView -= 0.1f;
            dragonGem.transform.position += Vector3.up * 0.05f;
            Vector3 rot = dragonGem.transform.rotation.eulerAngles;
            rot.y += 2f;
            dragonGem.transform.rotation = Quaternion.Euler(rot);
        }
        while (dragonGem.transform.localRotation.y < 0.99f)
        {
            yield return null;
            Vector3 rot = dragonGem.transform.rotation.eulerAngles;
            rot.y += 1f;
            dragonGem.transform.rotation = Quaternion.Euler(rot);
        }
        yield return new WaitForSeconds(1f);
        dragonGem.transform.position -= Vector3.up * 2.5f;
        dieSceneCam1.SetActive(false);
        dieSceneCam2.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseIn;
        portal.SetActive(true);
    }

    protected void SkillActive()
    {
        skillBress.Play();
    }

    #endregion
}

[RequireComponent(typeof(Rigidbody))]
public class BossCollider : MonoBehaviour
{
    Rigidbody rb;
    Boss bossScr;
    public GameObject parentObj;
    public int idx;
    public bool isDie;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        bossScr = parentObj.GetComponent<Boss>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
            bossScr.Damage(idx, 25);
        else if (collision.gameObject.CompareTag("Player"))
            bossScr.ColliderSet(idx);
    }


}


