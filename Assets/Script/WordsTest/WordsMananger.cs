﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsMananger : MonoBehaviour {
    public Sprite trueSprite;
    public Sprite falseSprite;
    public Sprite emptySprite;
    public GameObject[] tofList;
    public GameObject[] tofList2;
    private Image[] tofListImg2 = new Image[4]; 
    private Image[] tofListImg = new Image[4];

    public int maxPage = 8;
    //绑定左右选项
    public Text[] choicesRight;
    public Text[] choicesLeft;
    //绑定左右意思
    public Text meansLeft;
    public Text meansRight;
    private Book book;

    //英文单词储存
    public string[] choiceStrL;
    public string[] choiceStrR;
    //中文意思储存
    public string meansL;
    public string meansR;

    //储存选择列表引用
    public List<string[]> choiceList;
    //储存意思列表引用
    public List<string> meansList;
    //储存答案
    public List<int> ansList;
    public List<string> ansListStr;

    private int ansNumber = 0;

    public GameObject maskR;

    // Use this for initialization
    void Awake ()
    {

        book = GetComponent<Book>();
        InitChoiceList();
        choiceStrR = choiceList[0];
        choiceStrL = choiceList[1];
        meansL = meansList[0];
        meansR = meansList[1];
        SyncChoiceWithStrR();
        SyncChoiceWithStrL();
        for (int i = 0; i < 4; i++)
        {
            tofListImg[i] = tofList[i].GetComponent<Image>();
        }
        for (int i = 0; i < 4; i++)
        {
            tofListImg2[i] = tofList2[i].GetComponent<Image>();
        }
    }
	
    //初始化单词列表
    void InitChoiceList()
    {
        choiceList = new List<string[]>(4);
        choiceList.Add( new string[] { "pause", "challenge", "shear", "hang" });
        choiceList.Add( new string[] { "accommodate", "accomplish", "accord", "accessory" });
        choiceList.Add( new string[] { "allowance", "ambiguous", "alter", "allege" });
        choiceList.Add( new string[] { "coincide", "coarse", "bubble", "avail" });

        meansList = new List<string>(4);
        meansList.Add( "n.剪切，剪刀；vi.剪切，修剪，穿越；vt.剪去，剥夺");
        meansList.Add( "vt.容纳；向……提供住处；使适应");
        meansList.Add("vt.改变，变更，变动");
        meansList.Add( "adj.粗的，粗糙的；粗劣的；粗俗的");

        ansListStr.Add("shear");
        ansListStr.Add("accommodate");
        ansListStr.Add("alter");
        ansListStr.Add("coarse");

        int choiceListSize = choiceList.Count;
        for (int i = 0; i < choiceListSize; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if(choiceList[i][j] == ansListStr[i])
                {
                    ansList.Add(j);
                    break;
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {

	}

    //结束翻动执行
    public void OnEndFilp()
    {
        maskR.SetActive(false);
        meansL = meansR;
        SyncChoiceWithStrL();
        foreach (GameObject item in tofList2)
        {
            item.SetActive(false);
        }
        foreach (Image item in tofListImg2)
        {
            item.sprite = emptySprite;
        }
    }

    //开始翻动执行
    public void OnStartFilp()
    {
        foreach (GameObject item in tofList)
        {
            item.SetActive(false);
            
        }
        foreach (Image item in tofListImg)
        {
            item.sprite = emptySprite;
        }
        foreach (GameObject item in tofList2)
        {
            item.SetActive(true);
        }
        maskR.SetActive(true);
        //防止翻过头报错
        if (book.currentPage >= maxPage)
        {
            choiceStrL = choiceStrR;
            choiceStrR = new string[4];
            meansR = "";
            SyncChoiceWithStrL();
            SyncChoiceWithStrR();
            return;
        }
        choiceStrL = choiceStrR;
        choiceStrR = choiceList[book.currentPage / 2];
        meansR = meansList[book.currentPage / 2];

        SyncChoiceWithStrL();
        SyncChoiceWithStrR();

    }

    //同步左右选择项中文字，保持相同。
    private void SyncChoiceWithStrL()
    {
        meansRight.text = meansR;
        meansLeft.text = meansL;
        for (int i = 0; i < choiceStrR.Length; i++)
        {
            choicesLeft[i].text = choiceStrL[i];
        }
    }
    private void SyncChoiceWithStrR()
    {
        meansRight.text = meansR;
        meansLeft.text = meansL;
        for (int i = 0; i < choiceStrR.Length; i++)
        {
            choicesRight[i].text = choiceStrR[i];
        }
    }

    //用于处理点击后是否选择正确
    public void OnClickButton(int ans)
    {
        ansNumber = ansList[book.currentPage / 2 - 1];
        if (ans == ansNumber)
        {
            tofList[ans].SetActive(true);
            tofListImg[ans].sprite = trueSprite;
            tofListImg2[ans].sprite = trueSprite;
        }
        else
        {
            tofList[ans].SetActive(true);
            tofListImg[ans].sprite = falseSprite;
            tofListImg2[ans].sprite = falseSprite;
        }
    }
}
