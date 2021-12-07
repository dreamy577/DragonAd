using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    #region *변수
    public Transform effPos; //이펙트 위치 
    public ParticleSystem weaponEffect; //이펙트 파티클
    public playerHealth playerHealth; //playerHealth 스크립트 호출 

    public Transform firePos; //이펙트 (2) 위치
    public ParticleSystem fireEffect; //이펙트 (2) 파티클

    public Transform healPos; //힐이펙트 위치
    public ParticleSystem healEffect; //힐이펙트 파티클 

    public Transform stunPos; //스턴 이펙트 위치 
    public ParticleSystem stunEffect; //스턴 이펙트 파티클 

    Animator anim; //애니메이션 컴포넌트 

    public playerHealth ph;
    bool isDie; //플레이어 사망 감지 



    public enum MINMP { minmp = 5, }
    public enum MINMP2 { minmp2 = 10, }

    public Transform weaponPivot;
    public BoxCollider weaponCollider;

    #endregion


    void Start()
    {
        anim = this.GetComponent<Animator>();
        isDie = false; //die 비활성화로 시작 

    }

    void Update()
    {
        run();
        Attack();
        Heal();
        Stun();
        OnDamaged();
        OnPlayerDie();
    }


    private void run() //달리기 동작 및 애니메이션
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        if (!(h == 0 && v == 0)) //h와 v값이 0이 아닐때
        {
            anim.SetBool("isRun", true); //isRun 애니메이션 활성화
        }

        else
        {
            anim.SetBool("isRun", false);
        }
    }


    private void Attack()
    {
        //Normal Atack = 평타 _ 대미지 15
        if (Input.GetKeyDown(KeyCode.Q)) //Q키를 누르고있는 동안 일반 공격
        {
            anim.SetTrigger("isAttack_slice"); //일반 공격 애니메이션 활성화 
            //anim.SetTrigger("isAttack_normal"); //일반 공격 애니메이션 활성화 
        }

        //Skill Attack = 일반 스킬 공격 _ 대미지 20 > 스킬 (1)
        else if (Input.GetKeyDown(KeyCode.E)) //E키를 누르고있는 동안 가르기 공격
        {
            if (playerHealth.playerMp >= (int)MINMP.minmp) //플레이어의 MP가 5보다 크거나 같을때에만
            {
                anim.SetTrigger("isAttack_chop"); //찍기 애니메이션 활성화 
                partEff();
            }
        }

        //Slice Attack = 슬라이스 스킬 공격 _ 대미지 *2 > 스킬 (2)
        //else if (Input.GetKeyDown(KeyCode.Z)) //Z키를 누르고있는 동안 슬라이스 공격
        //{
        //    if (playerHealth.playerMp >= (int)MINMP.minmp) //플레이어의 MP가 5보다 크거나 같을때에만
        //    {
        //        anim.SetTrigger("isAttack_slice"); //슬라이스 애니메이션 활성화 
        //        playerHealth.playerMPDown(); //스킬 이펙트 활성화 시 플레이어 MP-5
        //    }
        //}

        //Pick Attack = 광역 스킬 공격 _ 범위 내 모든 대상 사망 > 스킬 (3)
        else if (Input.GetKeyDown(KeyCode.C)) //C키를 누르고있는 동안 픽 공격
        {
            if (playerHealth.playerMp >= (int)MINMP2.minmp2) //플레이어의 MP가 10보다 크거나 같을때에만 
            {
                fireEff(); //이펙트 코루틴 활성화 
                anim.SetTrigger("isAttack_pick"); //픽 애니메이션 활성화
            }
        }

        else //그 외의 상황에서는 Idle
        {
            anim.SetTrigger("Idle");
        }
    }

    private void Heal() //회복 스킬 = HP+25
    {
        if (Input.GetKeyDown(KeyCode.X)) //X키를 누르고있는 동안 힐
        {
            if (playerHealth.playerMp >= (int)MINMP2.minmp2) //mp가 10보다 크고
            {
                if (playerHealth.playerHp < playerHealth.hpmaxValue) // HP가 최대 HP보다 작을때 
                {
                    anim.SetTrigger("isHeal"); //힐 애니메이션 활성화 
                    StartCoroutine("healEff");
                    playerHealth.playerHPUp();
                }
            }
        }

        else
        {
            anim.SetTrigger("Idle");
        }
    }

    private void Stun()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (playerHealth.playerMp >= (int)MINMP2.minmp2) //플레이어의 MP가 10보다 크거나 같을때에만 
            {
                anim.SetTrigger("isStun");
                stunEff();
            }
        }

        else
        {
            anim.SetTrigger("Idle");
        }
    }

    public void OnDamaged() //플레이어 피격
    {
        if (gameObject.tag == "Enemy") //충돌 대상이 에너미일때
        {
            anim.SetTrigger("isDamaged"); //애니메이션 활성화 
        }
    }

    public void OnPlayerDie() //플레이어 사망 
    {
        if (ph.playerHp <= 0)
        {
            isDie = true;
            anim.SetTrigger("isDie");
        }
    }


    /*--------------------------------------------------------------------*/

    #region * 이펙트 코루틴

    public void partEff() //스킬 이펙트 = (1) 스킬 대응
    {
        weaponEffect.Play(); //이펙트 활성화
        //weaponEffect.gameObject.SetActive(true); //이펙트 활성화
        playerHealth.playerMPDown(); //스킬 이펙트 활성화 시 플레이어 MP -5
        //yield return new WaitForSeconds(0.2f); //0.2f동안 정지 
        //weaponEffect.gameObject.SetActive(false); //이펙트 비활성화 
    }

    public void fireEff() //스킬 이펙트 = (2) 스킬 대응 _tag=flame
    {
        //fireEffect.gameObject.SetActive(true); //이펙트 활성화
        fireEffect.Play(); //이펙트 활성화
        playerHealth.playerMPDown2(); //스킬 이펙트 활성화 시 플레이어 MP-10
        //yield return new WaitForSeconds(0.3f); //0.4f동안 정지
        //fireEffect.gameObject.SetActive(false); //이펙트 비활성화
    }

    public IEnumerator healEff() //힐
    {
        healEffect.gameObject.SetActive(true); //이펙트 활성화
        playerHealth.playerMPDown2(); //스킬 이펙트 활성화 시 플레이어 MP-10
        yield return new WaitForSeconds(0.3f);
        healEffect.gameObject.SetActive(false); //이펙트 비활성화 
    }

    public void stunEff() //스턴 
    {
        stunEffect.Play();
        //stunEffect.gameObject.SetActive(true);
        playerHealth.playerMPDown2();
        //yield return new WaitForSeconds(0.4f);
        //stunEffect.gameObject.SetActive(false);
    }

    #endregion


    public void AttackStart()
    {
        weaponCollider.enabled = true;
    }

    public void AttackEnd()
    {
        weaponCollider.enabled = false;
    }
}
