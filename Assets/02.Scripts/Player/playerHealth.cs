using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public Slider hpSlider; //hp��
    public Slider mpSlider; //mp��
    public Text HPtext;


    public int hpmaxValue; //�÷��̾��� �ִ� ü��
    public int playerHp; //�÷��̾��� ���� ü��

    public int mpmaxValue; //�÷��̾��� �ִ� ���ŷ�
    public int playerMp; //�÷��̾��� ���� ���ŷ� 

    bool isDie; //�÷��̾� ��� ���� 

    public enum HEALINGPOINT { healingpoint = 25 } //ȸ�� ��ġ 

    void Start()
    {
        isDie = false; //die ��Ȱ��ȭ�� ���� 
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

    private void playerHPDown() //�÷��̾� ü�� ����
    {
        if(playerHp>=1) //�÷��̾��� ü���� 1���� ũ�ų� ������
        {
            playerHp--; //�÷��̾��� ü�� ����
        }
        else if (playerHp <= 0)
        {
            OnPlayerDie();
        }
    }
    */

    public void playerMPDown() //�÷��̾� ���ŷ� ���� 
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



    public IEnumerator Recover() //���� �ڿ� ȸ�� 
    {
        while (isDie == false) //��� ��Ȱ��ȭ = ���������϶�
        {
            yield return new WaitForSeconds(3f); //3�� ���� �ο� = 3�ʿ� 1ȸ�� ȸ��

            if (playerHp < hpmaxValue) //���� ü���� �ִ� ü�º��� ������ 
            {
                playerHp += 1;//ü�� ȸ�� 
            }

            if (playerMp < mpmaxValue) //���� ���ŷ��� �ִ� ���ŷº��� ������ 
            {
                playerMp += 2; ;//���ŷ� ȸ��
            }
        }
    }

    public void Healing(string _case)
    {
        StartCoroutine(UseItemRecover(_case));
    }
   public IEnumerator UseItemRecover(string _case)
    {
        if (_case =="HP����")
        {
            while (playerMp < mpmaxValue) //���� ü���� �ִ� ü�º��� ������ 
            {
                playerMp += 5;//ü�� ȸ�� 
                yield return new WaitForSeconds(1f);
            }
        }
        else if (_case =="MP����")
        {
            while (playerMp < mpmaxValue) //���� ���ŷ��� �ִ� ���ŷº��� ������ 
            {
                playerMp += 2;//���ŷ� ȸ��
                yield return new WaitForSeconds(1f);
            }
        }
    }


}




