using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/*/*
public class ItemManager : MonoSingletone<ItemManager>
{
    [Header("플레이어")]
    public GameObject player;
    [Header("소지아이템 미니 바")]
    public Text coinText;
    [Header("SHOP OBJ")]
    public GameObject shop;
    private Dictionary<string, Sprite> tmp_inventorySlot = new Dictionary<string, Sprite>();//가방 열리기전에 임시 보관용
    [Header("자원부족 메세지")]
    public GameObject errorOBJ;
    public Image errorMassageBackGround;
    public Text errorMassage;
    [Header("구매완료 효과")]
    public Text[] DoneBuy;

    //퀵슬롯 아이템 종류 보관 배열
    public Items[] quickItems = new Items[5];

    //아이템 딕셔너리
    Dictionary<string, Items> itemDic = new Dictionary<string, Items>();

    //아이템 종류별 enum
    public enum Items
    {
        none = -1,
        hpPotion,
        mpPotion,
        mushRoom,
        axe,
        horn,        
    }

    [Header("퀵슬롯 아이템 보관 리스트")]
    //public int[] quickSlotArr ;
    //public List<int> quickSlotArr = new List<int>();
    private int quickCount = 0;
    public Image[] quickSlotImage;
    public Text[] quickSlotText;
    //아이템 스프라이트 보관소
    private Sprite[] itemSprites;

    #region * 소지품 관련 변수 모음    
    public int haveCoin = 0;
    public int haveMushroom = 0;
    public int haveHorn = 0;
    public int haveAxe = 0;

    public bool firstStone = false;
    public bool secStone = false;

    public const int HPPotionPrice = 100;
    public const int MPPotionPrice = 100;
    public const int FinalStonePrice = 50000;
    public int haveHPPotion = 0;
    public int haveMPPotion = 0;
    #endregion

    private void Awake()
    {
        SetCoin(2000);
        itemSprites = Resources.LoadAll<Sprite>("CollectionItem");
        for (int i = 0; i < quickItems.Length; i++)
        {
            quickItems[i] = Items.none;
        }
    }

    #region * 아이템 개수 관리 (Managing somethings) / 추후 추가 필요
    void SetCoin(int coin)
    {
        haveCoin = coin;
        coinText.text = haveCoin.ToString();
    }
    void UpdateCoin(int price)
    {
        haveCoin -= price;
        coinText.text = haveCoin.ToString();
    }
    void UpdateCollection()
    {
        haveMushroom -= 3;
        haveHorn -= 3;
    }
    #endregion

    #region * 상점 구매 (SHOP Buying)

    public void OcClickShopEsc()
    {
        shop.SetActive(false);
    }

    public void OnClickBuyItem_Slot1()
    {
        if (haveCoin >= HPPotionPrice)
        {
            UpdateCoin(HPPotionPrice);
            haveHPPotion++;
            AddQuickSlot(Items.hpPotion);
        }
        else
            StartCoroutine(Fade());
    }
    public void OnClickBuyItem_Slot2()
    {
        if (haveCoin >= MPPotionPrice)
        {
            UpdateCoin(MPPotionPrice);
            haveMPPotion++;
            AddQuickSlot(Items.mpPotion);
        }
        else
            StartCoroutine(Fade());
    }
    public void OnClickBuyItem_Slot3()
    {
        if (haveMushroom >= 3 && haveHorn >= 3)
        {
            UpdateCollection();
            firstStone = true;
        }
        else
            StartCoroutine(Fade());
    }

    public void OnClickBuyItem_Slot4()
    {
        if (haveAxe >= 50 && haveCoin >= FinalStonePrice)
        {
            UpdateCoin(FinalStonePrice);
            haveAxe -= 50;
        }
        else
            StartCoroutine(Fade());
    }
    public void AddQuickSlot(Items items)
    {
        switch (items)
        {
            case Items.hpPotion:
                FindEmptySlot(items, haveHPPotion); //퀵슬롯 빈자리에 이미지 넣어주고 갯수 텍스트 갱신
                break;
            case Items.mpPotion:
                FindEmptySlot(items, haveMPPotion);
                break;
            case Items.mushRoom:
                FindEmptySlot(items, haveMushroom);
                break;
            case Items.axe:
                FindEmptySlot(items, haveAxe);
                break;
            case Items.horn:
                FindEmptySlot(items, haveHorn);
                break;
        }
    }
    public void FindEmptySlot(Items items, int getItem) //퀵슬롯 빈자리 찾아서 이미지 넣어주기~
    {
        bool fineSamething = false;
        string tmpItemName = items.ToString();
        for (int i = 0; i < itemDic.Count; i++)
        {            
            if (itemDic.ContainsKey(tmpItemName))
            {
                quickSlotText[i].text = getItem.ToString();
                fineSamething = true;
                break;
            }
        }
        if(!fineSamething)
        {
            quickSlotImage[quickCount].color = new Color(1, 1, 1, 1);
            quickSlotImage[quickCount].sprite = itemSprites[(int)items];
            quickSlotText[quickCount].text = getItem.ToString();
            quickItems[quickCount] = items;
            itemDic.Add(tmpItemName, items);
            quickCount++;
        }
        //for (int i = 0; i < quickSlotArr.Count; i++)
        //{
        //    if (quickSlotArr[i] == (int)items) //이미 퀵슬롯에 같은 아이템이있다면
        //    {
        //        quickSlotText[i].text = getItem.ToString();
        //        fineSamething = true;
        //        break;
        //    }
        //}
        //if (!fineSamething) //같은게 없다면 그제서야 퀵슬롯 새로운 칸에 추가
        //{
        //    quickSlotImage[quickCount].color = new Color(1, 1, 1, 1);
        //    quickSlotImage[quickCount].sprite = itemSprites[(int)items];
        //    quickSlotText[quickCount].text = getItem.ToString();
        //    quickItems[quickCount] = items; 
        //    quickSlotArr.Add((int)items);
        //    quickCount++;
        //}
    }
    IEnumerator Fade()
    {
        errorOBJ.SetActive(true);
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += .1f;
            yield return new WaitForSeconds(.1f);
            errorMassageBackGround.color = new Color(0, 0, 0, fadeCount);
            if (fadeCount >= .2f)
                errorMassage.text = "자원이 부족합니다.";
        }
        while (fadeCount > 0f)
        {
            fadeCount -= .1f;
            yield return new WaitForSeconds(.1f);
            errorMassageBackGround.color = new Color(0, 0, 0, fadeCount);
            if (fadeCount <= .2f)
                errorMassage.text = "";
        }
        errorOBJ.SetActive(false);
    }

    #endregion

    #region * 퀵슬롯 사용(Using QuickSlot)
    /*
    어차피 퀵슬롯은 크기가 정해진 배열임.
    퀵슬롯배열에 있는 아이템 사용시 저장되어있는 값과 딕셔너리에 있는값이 일치하면
    그 아이템 효과만 발동해주면됨.
    퀵슬롯 배열에 저장되어있는 값은 이름 또는 items enum으로 비교하면 될것같음.
    굳이 튜플을 쓸 필요는 없지만 나중에 필요에따라 튜플도 고려.
    개수가 0이되면 배열에서 삭제 및 스프라이트 제거.
    */
    
    //public void QuickSlotUse()
    //{

    //}

    //public void QuickSlot1()
    //{
    //    CheckCanUseItem(0);
    //}
    //public void QuickSlot2()
    //{
    //    CheckCanUseItem(1);
    //}
    //public void QuickSlot3()
    //{
    //    CheckCanUseItem(2);
    //}
    //public void QuickSlot4()
    //{
    //    CheckCanUseItem(3);
    //}
    //public void QuickSlot5()
    //{
    //    CheckCanUseItem(4);
    //}
    //void CheckCanUseItem(int where)
    //{
    //    //if (quickSlotArr[where] == (int)Items.hpPotion)
    //    if()
    //    {
    //        if (haveHPPotion - 1 <= 0)
    //        {
    //            FindItemInQuickSlot(Items.hpPotion);
    //            haveHPPotion--;
    //            ResetQuickSlot(Items.hpPotion);
    //            quickSlotText[where].text = "";
    //            quickSlotArr.RemoveAt(where);                             
    //            return;
    //        }
    //        StartCoroutine(player.GetComponent<playerHealth>().UseItemRecover(ITEMKIND.HPPotion));          
    //        haveHPPotion--;
    //        quickSlotText[where].text = haveHPPotion.ToString();
    //    }
    //    else if(quickSlotArr[where] == (int)Items.mpPotion)
    //    {
    //        if (haveMPPotion - 1 <= 0)
    //        {
    //            FindItemInQuickSlot(Items.mpPotion);
    //            haveMPPotion--;
    //            ResetQuickSlot(Items.mpPotion);
    //            quickSlotText[where].text = "";
    //            quickSlotArr.RemoveAt(where);
    //            return;
    //        }
    //        StartCoroutine(player.GetComponent<playerHealth>().UseItemRecover(ITEMKIND.MPPotion));
    //        quickSlotText[where].text = haveMPPotion.ToString();
    //        haveMPPotion--;
    //    }
  //  }
  

/*
    void FindItemInQuickSlot(Items items)
    {
        for (int i = 0; i < quickItems.Length; i++)
        {
            if (quickItems[i] == items)
            {
                quickItems[i] = Items.none;
                return;
            }
        }
    }
    public void ResetQuickSlot(Items items)
    {
        for(int i =0; i<quickSlotImage.Length; i++)
        {
            quickSlotImage[i].sprite = null;
            quickSlotImage[i].color = new Color(.4f, .3f, .2f, .9f);
            quickSlotText[i].text = "";
        }
        for(int i = 0; i < quickItems.Length; i++)
        {
            if (quickItems[i] == items)
                quickSlotImage[i].sprite = itemSprites[(int)items];
        }
    }
   
    #endregion

    #region * 몬스터 처치시 골드/아이템 취득 관련 (Get Items/ Golds when hunt Monsters)

    //각 함수 매개변수에 증가할 갯수 입력만 해주시면 됩니다

    //몬스터 처치시 골드 획득
    public void GetGold(int addMoney)
    {
        haveCoin += addMoney;
        coinText.text = haveCoin.ToString();
    }

    //아이템 획득
    // public void AddQuickSlot(Items items); 함수 사용, 매개변수에 아이템 타입 입력
    #endregion
}
*/