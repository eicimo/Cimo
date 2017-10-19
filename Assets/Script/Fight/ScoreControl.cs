using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ScoreControl : MonoBehaviour
{
    public int MonsterScore;
    public int PlayerScore;
    public Animator Monster;
    public Animator Player;

    public Canvas canvas;
    
    public void OnClickBoom(PointerEventData id, int t) {
        var chosen = id.pointerCurrentRaycast.gameObject;
        var text = chosen.GetComponentInChildren<Text>();
        //清除气泡
        foreach (var item in boom) {
            DestroyImmediate(item);
            
        }

        if (text.text == pernouncationList[t]) {

            //PlayerScore += 10;
            Debug.Log("t!");
            StartCoroutine(WaitAndRight(t));
        }
        else {
            PlayerScore -= 5;
            Debug.Log("f!");
            StartCoroutine(WaitAndWrong(t));
        }

    }
    public float waitTime = 1;
    IEnumerator WaitAndRight(int t) {
        if (t + 1 >= pernouncationList.Count) {
            PlayerWin = true;
            PlayerScore += 10;
            Debug.Log("结束了！");
            //StopCoroutine("MonsterScoreUp");
            yield return new WaitForSeconds(waitTime);
            //StopCoroutine(MonsterScoreUp());
            //StartTurn();
            //yield break;
        }
        yield return new WaitForSeconds(waitTime);

        StartBoom(t + 1);
    }

    IEnumerator WaitAndWrong(int t) {
        yield return new WaitForSeconds(waitTime);

        StartBoom(t);
    }
    
    public int PosNum;                                          //选项数目
    void Awake() {
        for (int i = 0; i < PosNum; i++) {
            RectTransform t = PosDataObj[i].GetComponent<RectTransform>();
            posData[i] = t.position;        //position世界坐标,rect本地坐标
            qua = t.rotation;
        }
    }
    public int totalCount = 1;                                       //一共几个单词
    public string meansNow;                                        //当前中文意思
    public string word;                                                 //当前英文单词
    public List<string> pernouncationList;                  //音节列表
    List<List<string>> wrongs = new List<List<string>>();       //错误音节列表
    public List<string> wrongPernounList;                  //错误音节
    public GameObject boomPer;                                //气泡预设
    public List<GameObject> PosDataObj;
    public List<Vector3> posData;                              //储存选项位置
    public List<GameObject> boom;                           //气泡的引用
    Quaternion qua;
    public bool MonsterWin = false;
    public bool PlayerWin = false;

    void Start() {
        MonsterWin = false;
        PlayerWin = false;
        StartTurn();
    }
    int time = 0;
    void Update() {
        Monster.SetInteger("MonsterScore", MonsterScore);
        Monster.SetInteger("PlayerScore", PlayerScore);
        Player.SetInteger("MonsterScore", MonsterScore);
        Player.SetInteger("PlayerScore", PlayerScore);
        if (boom.Count > 4) {
            Debug.Log("?");
        }

        if(PlayerWin) {
            Debug.Log("PlayerWinNextTurn");
            PlayerWin = false;
            StartTurn();
            
        }

        if (MonsterWin) {
            Debug.Log("NextTurn");
            //清除气泡
            foreach (var item in boom) {
                DestroyImmediate(item);
            }
            MonsterWin = false;
            StartTurn();
            
            ////PlayerWin = false;
            //StartCoroutine(MonsterScoreUp());
            //StartCoroutine(WaitAndNext());
        }
        
    }
    IEnumerator WaitAndNext() {
        yield return new WaitForSeconds(waitTime);
        StartBoom(time);
        /*
         * 
         * 这里因为只有一组数据才选择time，若有多组更改为++time
         * 
         * 
         * */
    }
    //控制怪物分数自然增长
    IEnumerator MonsterScoreUp() {
        for (int i = 0; i < 10; i++) {
            if (PlayerWin) {
                Debug.Log("MonsterScoreUpEnd");
                PlayerWin = false;
                yield break;
            }
            yield return new WaitForSeconds(1);
            if (PlayerWin) {
                Debug.Log("MonsterScoreUpEnd");
                PlayerWin = false;
                yield break;
            }
        }
        MonsterScore += 10;
        MonsterWin = true;
    }

    //开始某一回合
    void StartTurn() {
        InitList(0);                                                       //初始化本回合列表
        StartCoroutine(MonsterScoreUp());               //开始一个计算怪物倒计时的协程
        StartBoom(time);                                           //实例化气泡

    }


    void StartBoom(int times) {
        foreach (var item in boom) {
            DestroyImmediate(item);

        }
        boom.Clear();
        if (times >= pernouncationList.Count) {
            //PlayerWin = true;
            //PlayerScore += 10;
            return;
        }
        GetWrongList(0);                                            //初始化错误列表
        int numid = Random.Range(0, PosNum);
        for (int i = 0; i < PosNum; i++) {
            //实例化气泡
            GameObject bom = Instantiate(boomPer, posData[i], qua, canvas.transform);
            Text t = bom.GetComponentInChildren<Text>();
            EventTrigger evt = bom.GetComponent<EventTrigger>();
            var trigs = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            trigs.callback.AddListener((data) => { OnClickBoom((PointerEventData)data, times); });

            evt.triggers.Add(trigs);
            boom.Add(bom);
            if (i == numid) {
                t.text = pernouncationList[times];
            }
            else {
                t.text = wrongs[times][i];
            }
        }

    }

    void InitList(int id) {
        pernouncationList.Clear();
        meansNow = "交流";
        word = "communication";
        pernouncationList.Add("com");
        pernouncationList.Add("mu");
        pernouncationList.Add("ni");
        pernouncationList.Add("ca");
        pernouncationList.Add("tion");
    }

    void GetWrongList(int id) {

        wrongs.Add(new List<string>
        {
            "aa",
            "eqw",
            "weq",
            "ww"
        });
        wrongs.Add(new List<string>
        {
            "ew",
            "oos",
            "aas",
            "wqe"
        });
        wrongs.Add(new List<string>
        {
            "wsf",
            "esd",
            "asdx",
            "aas"
        });
        wrongs.Add(new List<string>
        {
            "ssd",
            "wgf",
            "ui",
            "ew"
        });
        wrongs.Add(new List<string>
        {
            "bf",
            "sdu",
            "hb",
            "qw"
        });
    }
}
