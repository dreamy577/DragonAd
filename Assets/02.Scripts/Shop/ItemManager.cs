using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/*/*
public class ItemManager : MonoSingletone<ItemManager>
{
    [Header("�÷��̾�")]
    public GameObject player;
    [Header("���������� �̴� ��")]
    public Text coinText;
    [Header("SHOP OBJ")]
    public GameObject shop;
    private Dictionary<string, Sprite> tmp_inventorySlot = new Dictionary<string, Sprite>();//���� ���������� �ӽ� ������
    [Header("�ڿ����� �޼���")]
    public GameObject errorOBJ;
    public Image errorMassageBackGround;
    public Text errorMassage;
    [Header("���ſϷ� ȿ��")]
    public Text[] DoneBuy;

    //������ ������ ���� ���� �迭
    public Items[] quickItems = new Items[5];

    //������ ��ųʸ�
    Dictionary<string, Items> itemDic = new Dictionary<string, Items>();

    //������ ������ enum
    public enum Items
    {
        none = -1,
        hpPotion,
        mpPotion,
        mushRoom,
        axe,
        horn,        
    }

    [Header("������ ������ ���� ����Ʈ")]
    //public int[] quickSlotArr ;
    //public List<int> quickSlotArr = new List<int>();
    private int quickCount = 0;
    public Image[] quickSlotImage;
    public Text[] quickSlotText;
    //������ ��������Ʈ ������
    private Sprite[] itemSprites;

    #region * ����ǰ ���� ���� ����    
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

    #region * ������ ���� ���� (Managing somethings) / ���� �߰� �ʿ�
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

    #region * ���� ���� (SHOP Buying)

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
                FindEmptySlot(items, haveHPPotion); //������ ���ڸ��� �̹��� �־��ְ� ���� �ؽ�Ʈ ����
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
    public void FindEmptySlot(Items items, int getItem) //������ ���ڸ� ã�Ƽ� �̹��� �־��ֱ�~
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
        //    if (quickSlotArr[i] == (int)items) //�̹� �����Կ� ���� ���������ִٸ�
        //    {
        //        quickSlotText[i].text = getItem.ToString();
        //        fineSamething = true;
        //        break;
        //    }
        //}
        //if (!fineSamething) //������ ���ٸ� �������� ������ ���ο� ĭ�� �߰�
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
                errorMassage.text = "�ڿ��� �����մϴ�.";
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

    #region * ������ ���(Using QuickSlot)
    /*
    ������ �������� ũ�Ⱑ ������ �迭��.
    �����Թ迭�� �ִ� ������ ���� ����Ǿ��ִ� ���� ��ųʸ��� �ִ°��� ��ġ�ϸ�
    �� ������ ȿ���� �ߵ����ָ��.
    ������ �迭�� ����Ǿ��ִ� ���� �̸� �Ǵ� items enum���� ���ϸ� �ɰͰ���.
    ���� Ʃ���� �� �ʿ�� ������ ���߿� �ʿ信���� Ʃ�õ� ���.
    ������ 0�̵Ǹ� �迭���� ���� �� ��������Ʈ ����.
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

    #region * ���� óġ�� ���/������ ��� ���� (Get Items/ Golds when hunt Monsters)

    //�� �Լ� �Ű������� ������ ���� �Է¸� ���ֽø� �˴ϴ�

    //���� óġ�� ��� ȹ��
    public void GetGold(int addMoney)
    {
        haveCoin += addMoney;
        coinText.text = haveCoin.ToString();
    }

    //������ ȹ��
    // public void AddQuickSlot(Items items); �Լ� ���, �Ű������� ������ Ÿ�� �Է�
    #endregion
}
*/