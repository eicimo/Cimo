using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ScoreControl : MonoBehaviour
{
    public GameObject textEar;
    public GameObject textPlayer;
    TextMesh textEarT;
    TextMesh textPlayerT;

    public int MonsterScore;
    public int PlayerScore;
    public Animator Monster;
    public Animator Player;

    public Canvas canvas;
    public float emptyWaitTime = 0.5f;

    /// <summary>
    /// 点击气泡会触发的事件
    /// </summary>
    /// <param name="id">点击物的数据</param>
    /// <param name="t">回合数</param>
    public void OnClickBoom(PointerEventData id, int t) {
        var chosen = id.pointerCurrentRaycast.gameObject;
        var text = chosen.GetComponentInChildren<Text>();
        // 清除气泡
        foreach (var item in boom) {

            if (item != chosen) StartCoroutine(WaitAndDestroy(item));
            else StartCoroutine(WaitAndDestroyMaster(item));
        }
        // 如果回答正确
        if (text.text == pernouncationList[t]) {
            Debug.Log("t!");
            StartCoroutine(WaitAndRight(t));
        }
        else {
            PlayerScore -= 5;
            Debug.Log("f!");
            StartCoroutine(WaitAndWrong(t));
        }

    }


    /// <summary>
    /// 拼对一个音节的处理函数
    /// </summary>
    /// <param name="t">回合数</param>
    /// <returns></returns>
    IEnumerator WaitAndRight(int t) {
        // 如果整个单词都拼对了
        if (t + 1 >= pernouncationList.Count) {
            StartBoom(t + 1);
            yield break;
        }
        yield return new WaitForSeconds(waitTime + emptyWaitTime);

        StartBoom(t + 1);
    }

    /// <summary>
    /// 点击后拼写错误的处理函数
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator WaitAndWrong(int t) {
        yield return new WaitForSeconds(waitTime + emptyWaitTime);

        StartBoom(t);
    }

    /// <summary>
    /// 等待一段时间后执行无参函数
    /// </summary>
    /// <param name="waitTime">等待的时间</param>
    /// <param name="w">需要执行的无参函数</param>
    /// <returns></returns>
    IEnumerator WaitAndToDoSth(float waitTime, JustDo w) {
        yield return new WaitForSeconds(waitTime);
        w();
    }


    /// <summary>
    /// 使气泡缩小并消失
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    IEnumerator WaitAndDestroy(GameObject obj) {
        for (int i = 0; i < 8; i++) {
            yield return new WaitForSeconds(waitTime / 20);
            if (obj) obj.transform.localScale -= new Vector3(0.06f, 0.06f, 0);
        }
        DestroyImmediate(obj);
    }

    /// <summary>
    /// 使选择正确的气泡缩小并消失。
    /// </summary>
    /// <param name="obj">气泡</param>
    /// <returns></returns>
    IEnumerator WaitAndDestroyMaster(GameObject obj) {
        for (int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(waitTime / 20);
            if (obj) obj.transform.localScale += new Vector3(0.02f, 0.02f, 0);
        }
        for (int i = 0; i < 8; i++) {
            yield return new WaitForSeconds(waitTime / 20);
            if (obj) obj.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        }
        DestroyImmediate(obj);
    }

    public int PosNum;                                          //选项数目
    void Awake() {

        for (int i = 0; i < PosNum; i++) {
            RectTransform t = PosDataObj[i].GetComponent<RectTransform>();
            posData[i] = t.position;        //position世界坐标,rect本地坐标
            qua = t.rotation;
        }
    }


    void Start() {

        textEarT = textEar.GetComponent<TextMesh>();
        textPlayerT = textPlayer.GetComponent<TextMesh>();
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
        if (textEarT.text != MonsterScore.ToString()) {
            textEarT.text = MonsterScore.ToString();
        }
        if (textPlayerT.text != PlayerScore.ToString()) {
            textPlayerT.text = PlayerScore.ToString();
        }
        if (boom.Count > 4) {
            Debug.Log("?");
        }

    }


    IEnumerator MonsterScoreUpV;
    /// <summary>
    /// 控制怪物分数自然增长
    /// </summary>
    /// <returns></returns>
    IEnumerator MonsterScoreUp() {
        for (int i = 0; i < 10; i++) {
            if (PlayerWin) yield break;
            yield return new WaitForSeconds(monsterWaitTime / 10);
        }
        if (PlayerWin) yield break;
        else {
            MonsterScore += 10;
            MonsterWin = true;
        }
        //清除气泡        
        MonsterWin = false;
        PlayerWin = false;
        foreach (var item in boom) {
            StartCoroutine(WaitAndDestroy(item));
        }


        StartCoroutine(WaitAndToDoSth(waitTime + emptyWaitTime, StartTurn));
    }


    /// <summary>
    /// 开始某一回合。回合数由 time控制。
    /// </summary>
    void StartTurn() {
        if (turn >= maxTurn) {
            Debug.Log("游戏结束");
            return;
        }
        InitList(0);                                                       //初始化本回合列表
        PlayerWin = false;
        MonsterScoreUpV = MonsterScoreUp();
        StartCoroutine(MonsterScoreUpV);
        StartBoom(time);


    }

    /// <summary>
    /// 用于无参的函数的委托。
    /// </summary>
    delegate void JustDo();

    /// <summary>
    /// 开始一个音节拼写单元
    /// </summary>
    /// <param name="times">回合数</param>
    void StartBoom(int times) {
        // 立即删除所有气泡
        JustDo DeleteAll = () =>
        {
            foreach (var item in boom) {
                if (item) DestroyImmediate(item);

            }
            boom.Clear();
        };

        // 如果单词拼对了
        if (times >= pernouncationList.Count) {
            PlayerScore += 10;
            PlayerWin = true;
            StopCoroutine(MonsterScoreUpV);
            Debug.Log("结束了！");
            turn++;
            StartCoroutine(WaitAndToDoSth(waitTime, DeleteAll));
            StartCoroutine(WaitAndToDoSth(waitTime + emptyWaitTime, StartTurn));

            return;
        }
        DeleteAll();

        //初始化错误列表
        GetWrongList(0);
        int numid = Random.Range(0, PosNum);
        for (int i = 0; i < PosNum; i++) {
            // 实例化气泡
            GameObject bom = Instantiate(boomPer, posData[i], qua, canvas.transform);
            Text t = bom.GetComponentInChildren<Text>();

            // 注册点击事件
            EventTrigger evt = bom.GetComponent<EventTrigger>();
            var trigs = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            trigs.callback.AddListener((data) => { OnClickBoom((PointerEventData)data, times); });
            evt.triggers.Add(trigs);

            // 显示气泡上的文字
            boom.Add(bom);
            if (i == numid) {
                t.text = pernouncationList[times];
            }
            else {
                t.text = wrongs[times][i];
            }
        }

    }

    /// <summary>
    /// 初始化每一回合的单词内容和音节列表
    /// </summary>
    /// <param name="id">回合数</param>
    void InitList(int id) {
        // TODO —— 根据回合数生成不同的列表
        pernouncationList.Clear();
        meansNow = "交流";
        word = "communication";
        pernouncationList.Add("com");
        pernouncationList.Add("mu");
        pernouncationList.Add("ni");
        pernouncationList.Add("ca");
        pernouncationList.Add("tion");
    }

    /// <summary>
    /// 根据回合数生成与正确音节同时出现的错误音节
    /// </summary>
    /// <param name="id">回合数</param>
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


    /// <summary>
    /// 一共几个单词
    /// </summary>
    public int totalCount = 1;
    /// <summary>
    /// 当前中文意思
    /// </summary>
    public string meansNow;
    /// <summary>
    /// 当前英文单词
    /// </summary>
    public string word;
    /// <summary>
    /// 音节列表
    /// </summary>
    public List<string> pernouncationList;
    /// <summary>
    /// 错误音节列表的列表
    /// </summary>
    List<List<string>> wrongs = new List<List<string>>();
    /// <summary>
    /// 错误音节列表
    /// </summary>
    public List<string> wrongPernounList;
    /// <summary>
    /// 气泡预设
    /// </summary>
    public GameObject boomPer;
    /// <summary>
    /// 气泡位置的标识物体
    /// </summary>
    public List<GameObject> PosDataObj;
    /// <summary>
    /// 储存选项位置
    /// </summary>
    public List<Vector3> posData;
    /// <summary>
    /// 气泡的引用
    /// </summary>
    public List<GameObject> boom;
    Quaternion qua;
    public bool MonsterWin = false;
    public bool PlayerWin = false;

    /// <summary>
    /// 第一部分的等待时间
    /// </summary>
    public float waitTime = 1;

    public int turn = 0;
    public int maxTurn = 10;
    /// <summary>
    /// 本回合拼写怪物的回答时间（限制）
    /// </summary>
    public float monsterWaitTime = 8f;
}
