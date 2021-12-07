using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public Slider hpSlider; //hp바
    public Slider mpSlider; //mp바
    public Text HPtext;


    public int hpmaxValue; //플레이어의 최대 체력
    public int playerHp; //플레이어의 현재 체력

    public int mpmaxValue; //플레이어의 최대 정신력
    public int playerMp; //플레이어의 현재 정신력 

    bool isDie; //플레이어 사망 감지 

    public enum HEALINGPOINT { healingpoint = 25 } //회복 수치 

    void Start()
    {
        isDie = false; //die 비활성화로 시작 
        StartCoroutine(Recover());
    }

    void Update()
    {
        //HP
        hpSlider.maxValue = hpmaxValue;
        hpSlider.value = playerHp;
        HPtext.text = playerHp.ToString() + " / 100 " ;

        //MP
        mpSlider.maxValue = mpmaxValue;
        mpSlider.value = playerMp;

    }
    /*

    private void playerHPDown() //플레이어 체력 제어
    {
        if(playerHp>=1) //플레이어의 체력이 1보다 크거나 같을때
        {
            playerHp--; //플레이어의 체력 차감
        }
        else if (playerHp <= 0)
        {
            OnPlayerDie();
        }
    }
    */

    public void playerMPDown() //플레이어 정신력 제어 
    {
        playerMp -= 5;
    }

    public void playerMPDown2()
    {
        playerMp -= 10;
    }

    public void playerHPUp()
    {
        playerHp += (int)HEALINGPOINT.healingpoint;
    }



    public IEnumerator Recover() //스탯 자연 회복 
    {
        while (isDie == false) //사망 비활성화 = 생존상태일때
        {
            yield return new WaitForSeconds(3f); //3초 지연 부여 = 3초에 1회씩 회복

            if (playerHp < hpmaxValue) //현재 체력이 최대 체력보다 작을때 
            {
                playerHp += 1;//체력 회복 
            }

            if (playerMp < mpmaxValue) //현재 정신력이 최대 정신력보다 작을때 
            {
                playerMp += 2; ;//정신력 회복
            }
        }
    }

    public void Healing(string _case)
    {
        StartCoroutine(UseItemRecover(_case));
    }
   public IEnumerator UseItemRecover(string _case)
    {
        if (_case =="HP포션")
        {
            while (playerMp < mpmaxValue) //현재 체력이 최대 체력보다 작을때 
            {
                playerMp += 5;//체력 회복 
                yield return new WaitForSeconds(1f);
            }
        }
        else if (_case =="MP포션")
        {
            while (playerMp < mpmaxValue) //현재 정신력이 최대 정신력보다 작을때 
            {
                playerMp += 2;//정신력 회복
                yield return new WaitForSeconds(1f);
            }
        }
    }


}




