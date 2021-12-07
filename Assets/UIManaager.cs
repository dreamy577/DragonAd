using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemKind: UIManaager
{
    private string name;
    private int effect;
    private Sprite sprite;
    public ItemKind(string _name, int _effect, Sprite _sprite)
    {
        this.name = _name;
        this.effect = _effect;
        this.sprite = _sprite;
    }
}
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
public class UIManaager : Monosingleton<UIManaager>
{
    [Header("�÷��̾�")]
    public GameObject player;
    [Header("���� ���� �ؽ�Ʈ")]
    public Text coinText;
    [Header("Shop OBJ")]
    public GameObject shop;
    [Header("�ڿ����� �޼���")]
    public GameObject errorOBJ;
    public Image errorMassageBackGround;
    public Text errorMassage;
    [Header("���ſϷ� ȿ��")] //���߿� �߰�/������
    public Text[] DoneBuy;

    //���� �� ��ü �Ҹ� ������ ������ ��ųʸ�
    public Dictionary<string, ItemKind> itemDic = new Dictionary<string, ItemKind>();
    [Header ("������ ������ �̹��� /  ���� �ؽ�Ʈ")]
    public Image[] quickSlotImage;
    public Text[] quickSlotText;
    [Header("������ ��������Ʈ ���� �迭")]
    public Sprite[] itemSprites;

    //������ ��ü �迭
    public string []qucikArr_ = new string [5];

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
    #region * ������ ȿ��
    public int hpEffect = 20;
    public int mpEffect = 20;
    #endregion
    private void Awake()
    {
        itemSprites = Resources.LoadAll<Sprite>("CollectionItem"); //������ ��������Ʈ ��ü �ε� �ص�
        SetCoin(2000);        
        SetItemDictionary();
        for (int i = 0; i < qucikArr_.Length; i++) 
        {
            qucikArr_[i] = null;
        }
    }    
    void SetItemDictionary()
    {
        string name;
        name = "HP����";
        itemDic.Add(name, new ItemKind(name, hpEffect, itemSprites[0]));
        name = "MP����";
        itemDic.Add(name, new ItemKind(name, mpEffect, itemSprites[1]));
        name = "��ȭ�� ����";
        itemDic.Add(name, new ItemKind(name, 0, itemSprites[2]));
        name = "������ ����";
        itemDic.Add(name, new ItemKind(name, 0, itemSprites[3]));
        name = "����� ����";
        itemDic.Add(name, new ItemKind(name, 0, itemSprites[4]));
    }
    #region * ������ ���� ���� (Managing somethings)
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

    #region * ���� ����� ���� ���� ����
    public void GetCoin()
    {
        int i = UnityEngine.Random.Range(100, 500);
        haveCoin += i;
        coinText.text = haveCoin.ToString();
    }
    #endregion

    #region * ���� ���� (SHOP Buying)
    public void OcClickShopEsc() //���� OFF
    {
        shop.SetActive(false);
    }
    public void OnClickBuyItem_Slot1()
    {
        if(haveCoin >= HPPotionPrice)
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
    #endregion

    #region * �ڿ����� �޼��� ���̵���
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

    #region * ������ 
    public void AddQuickSlot(Items items)
     {
        switch (items)
        {
            case Items.hpPotion:
                FindEmptySlot("HP����", haveHPPotion, items); //������ ���ڸ��� �̹��� �־��ְ� ���� �ؽ�Ʈ ����
                break;
            case Items.mpPotion:
                FindEmptySlot("MP����", haveMPPotion, items);
                break;
            case Items.mushRoom:
                FindEmptySlot("��ȭ�� ����", haveMushroom, items);
                break;
            case Items.axe:
                FindEmptySlot("������ ����", haveAxe, items);
                break;
            case Items.horn:
                FindEmptySlot("����� ����", haveHorn, items);
                break;
        }
    }
    public void FindEmptySlot(string itemname, int getItem, Items items)
    {
        bool fineSamething = false;

        for (int i = 0; i < qucikArr_.Length; i++)
        {
            if (qucikArr_[i] == itemname) //�̹� ������ �����Կ� �ִ��� ã��
            {
                quickSlotText[i].text = getItem.ToString();
                fineSamething = true;
                break;
            }
        }
        if (!fineSamething) //���ٸ�
        {
            for (int i = 0; i < qucikArr_.Length; i++)
            {
                if (qucikArr_[i] == null)
                {
                    quickSlotImage[i].color = new Color(1, 1, 1, 1);
                    quickSlotImage[i].sprite = itemSprites[(int)items];
                    quickSlotText[i].text = getItem.ToString();
                    qucikArr_[i] = itemname;
                    break;
                }
            }
        }
    }
    #endregion

    #region * ������ ���

    public void UseQuick_1()
    {
        checkWhatItem(0);
    }
    public void UseQuick_2()
    {
        checkWhatItem(1);
    }
    public void UseQuick_3()
    {
        checkWhatItem(2);
    }
    public void UseQuick_4()
    {
        checkWhatItem(3);
    }
    public void UseQuick_5()
    {
        checkWhatItem(4);
    }

    void checkWhatItem(int num)
    { 
        switch(qucikArr_[num]) //�ٽ����� ������Ե� �ؾ���
        {
            case "HP����":
                if (haveHPPotion-1 > 0)
                {
                    haveHPPotion--;
                    quickSlotText[num].text = haveHPPotion.ToString();
                    player.GetComponent<playerHealth>().Healing("HP����");
                }
                else
                {
                    quickSlotText[num].text = "";
                    quickSlotImage[num].sprite = null;
                    quickSlotImage[num].color = new Color(.4f, .3f, .2f, .9f);
                    qucikArr_[num] = null;
                    haveHPPotion = 0;
                }
                break;
            case "MP����":
                if (haveMPPotion-1 > 0)
                {
                    haveMPPotion--;
                    quickSlotText[num].text = haveMPPotion.ToString();
                    player.GetComponent<playerHealth>().Healing("MP����");
                }
                else
                {
                    quickSlotText[num].text = "";
                    quickSlotImage[num].sprite = null;
                    quickSlotImage[num].color = new Color(.4f, .3f, .2f, .9f);
                    qucikArr_[num] = null;
                    haveMPPotion = 0;
                }
                break;
        }
    }
    #endregion


}
