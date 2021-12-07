using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollider : MonoBehaviour
{
    public playerHealth ph;
    public UIManaager im;
    public BoxCollider bc;
    public Image damageScreen;


    private void Start()
    {
        bc = this.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") //�浹ü �±װ� ���ʹ��϶�
        {
            ph.playerHp -= 10; //�÷��̾� ü��-10         
            StartCoroutine(showBloodScreen());
        }

        //else if (other.gameObject.tag == "HPPotion") //충돌 ?�?�이 HP?�션?�때 
        //{
        //    im.haveHPPotion++;
        //}

        //else if (other.gameObject.tag == "MPPotion") //충돌 ?�?�에 MP?�션?�때 
        //{
        //    im.haveMPPotion++;
        //}

        //else if (other.gameObject.tag == "Horn")
        //{
        //    im.haveHorn++;
        //}

        else if (other.gameObject.tag == "Mushroom")
        {
            im.haveMushroom++;
            UIManaager.Instance.AddQuickSlot(Items.mushRoom);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "Axe")
        {
            im.haveAxe++;
            UIManaager.Instance.AddQuickSlot(Items.axe);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.tag == "Coin")
        {
            im.haveCoin++;
        }

        else if(other.gameObject.tag=="Horn")
        {
            Destroy(other.gameObject);
        }
    }

    //�׽��ÿ� -  �ݸ��� üũ�ȵ�
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Mushroom")
    //    {
    //        Destroy(collision.gameObject);
    //        im.haveMushroom++;
    //        UIManaager.Instance.AddQuickSlot(Items.mushRoom);
    //    }

    //    else if (collision.gameObject.tag == "Axe")
    //    {
    //        Destroy(collision.gameObject);
    //        im.haveAxe++;
    //        UIManaager.Instance.AddQuickSlot(Items.axe);
    //    }

    //}

    IEnumerator showBloodScreen()
    {
        damageScreen.color = new Color(125 / 255f, 0, 0, Random.Range(0.5f, 0.7f));

        yield return new WaitForSeconds(0.25f);
        damageScreen.color = Color.clear;
    }

}


