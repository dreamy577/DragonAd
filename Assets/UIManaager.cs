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
public class UIManaager : Monosingleton<UIManaager>
{
    [Header("플레이어")]
    public GameObject player;
    [Header("소지 코인 텍스트")]
    public Text coinText;
    [Header("Shop OBJ")]
    public GameObject shop;
    [Header("자원부족 메세지")]
    public GameObject errorOBJ;
    public Image errorMassageBackGround;
    public Text errorMassage;
    [Header("구매완료 효과")] //나중에 추가/수정필
    public Text[] DoneBuy;

    //게임 내 전체 소모성 아이템 종류별 딕셔너리
    public Dictionary<string, ItemKind> itemDic = new Dictionary<string, ItemKind>();
    [Header ("퀵슬롯 아이템 이미지 /  개수 텍스트")]
    public Image[] quickSlotImage;
    public Text[] quickSlotText;
    [Header("아이템 스프라이트 보관 배열")]
    public Sprite[] itemSprites;

    //퀵슬롯 자체 배열
    public string []qucikArr_ = new string [5];

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
    #region * 아이템 효과
    public int hpEffect = 20;
    public int mpEffect = 20;
    #endregion
    private void Awake()
    {
        itemSprites = Resources.LoadAll<Sprite>("CollectionItem"); //아이템 스프라이트 전체 로드 해둠
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
        name = "HP포션";
        itemDic.Add(name, new ItemKind(name, hpEffect, itemSprites[0]));
        name = "MP포션";
        itemDic.Add(name, new ItemKind(name, mpEffect, itemSprites[1]));
        name = "정화된 버섯";
        itemDic.Add(name, new ItemKind(name, 0, itemSprites[2]));
        name = "정령의 도끼";
        itemDic.Add(name, new ItemKind(name, 0, itemSprites[3]));
        name = "응축된 정수";
        itemDic.Add(name, new ItemKind(name, 0, itemSprites[4]));
    }
    #region * 아이템 개수 관리 (Managing somethings)
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

    #region * 몬스터 사망시 랜덤 코인 증가
    public void GetCoin()
    {
        int i = UnityEngine.Random.Range(100, 500);
        haveCoin += i;
        coinText.text = haveCoin.ToString();
    }
    #endregion

    #region * 상점 구매 (SHOP Buying)
    public void OcClickShopEsc() //상점 OFF
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

    #region * 자원부족 메세지 페이드인
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

    #region * 퀵슬롯 
    public void AddQuickSlot(Items items)
     {
        switch (items)
        {
            case Items.hpPotion:
                FindEmptySlot("HP포션", haveHPPotion, items); //퀵슬롯 빈자리에 이미지 넣어주고 갯수 텍스트 갱신
                break;
            case Items.mpPotion:
                FindEmptySlot("MP포션", haveMPPotion, items);
                break;
            case Items.mushRoom:
                FindEmptySlot("정화된 버섯", haveMushroom, items);
                break;
            case Items.axe:
                FindEmptySlot("정령의 도끼", haveAxe, items);
                break;
            case Items.horn:
                FindEmptySlot("응축된 정수", haveHorn, items);
                break;
        }
    }
    public void FindEmptySlot(string itemname, int getItem, Items items)
    {
        bool fineSamething = false;

        for (int i = 0; i < qucikArr_.Length; i++)
        {
            if (qucikArr_[i] == itemname) //이미 같은게 퀵슬롯에 있는지 찾음
            {
                quickSlotText[i].text = getItem.ToString();
                fineSamething = true;
                break;
            }
        }
        if (!fineSamething) //없다면
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

    #region * 아이템 사용

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
        switch(qucikArr_[num]) //다썼을때 사라지게도 해야함
        {
            case "HP포션":
                if (haveHPPotion-1 > 0)
                {
                    haveHPPotion--;
                    quickSlotText[num].text = haveHPPotion.ToString();
                    player.GetComponent<playerHealth>().Healing("HP포션");
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
            case "MP포션":
                if (haveMPPotion-1 > 0)
                {
                    haveMPPotion--;
                    quickSlotText[num].text = haveMPPotion.ToString();
                    player.GetComponent<playerHealth>().Healing("MP포션");
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
