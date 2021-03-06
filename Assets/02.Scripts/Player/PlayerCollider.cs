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
        if (other.gameObject.tag == "Enemy") //충돌체 태그가 에너미일때
        {
            ph.playerHp -= 10; //플레이어 체력-10         
            StartCoroutine(showBloodScreen());
        }

        //else if (other.gameObject.tag == "HPPotion") //異⑸룎 ???곸씠 HP?ъ뀡?쇰븣 
        //{
        //    im.haveHPPotion++;
        //}

        //else if (other.gameObject.tag == "MPPotion") //異⑸룎 ???곸뿉 MP?ъ뀡?쇰븣 
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

    //테스팅용 -  콜리전 체크안됨
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


