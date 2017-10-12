using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsMananger : MonoBehaviour {
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
    }
	
    //初始化单词列表
    void InitChoiceList()
    {
        choiceList = new List<string[]>(4);
        choiceList.Add( new string[] { "pause", "challenge", "shear", "hang" });
        choiceList.Add( new string[] { "1", "2", "3", "4" });
        choiceList.Add( new string[] { "m", "n", "p", "q" });
        choiceList.Add( new string[] { "e", "d", "p", "g" });

        meansList = new List<string>(4);
        meansList.Add( "n.剪切，剪刀；vi.剪切，修剪，穿越；vt.剪去，剥夺");
        meansList.Add( "number-1");
        meansList.Add("mnpq-n");
        meansList.Add( "edfg-p");

        ansListStr.Add("shear");
        ansListStr.Add("1");
        ansListStr.Add("n");
        ansListStr.Add("p");

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
        meansL = meansR;
        SyncChoiceWithStrL();
    }

    //开始翻动执行
    public void OnStartFilp()
    {
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
        if (ans == ansNumber) Debug.Log("true");
        else Debug.Log("false");
    }
}
