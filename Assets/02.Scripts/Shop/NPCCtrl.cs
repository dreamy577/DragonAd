using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCCtrl : MonoBehaviour
{
    public GameObject shopSlot;

    #region * NPC 아웃라인 온오프(NPC Outline ON/OFF)
    private void OnMouseOver()
    {        
        OutLineONOFF(true);
    }
    private void OnMouseExit()
    {
        OutLineONOFF(false);
    }
    void OutLineONOFF(bool check)
    {
        if (check == true)
            this.gameObject.GetComponent<Outline>().enabled = true;
        else
            this.gameObject.GetComponent<Outline>().enabled = false;
    }
    #endregion

    #region * 상점 온오프(Shop ON/OFF)
    private void OnMouseDown()
    {
        shopSlot.gameObject.SetActive(true);
    }
    public void OnClickESC()
    {
        shopSlot.gameObject.SetActive(false);
    }
    #endregion

}
