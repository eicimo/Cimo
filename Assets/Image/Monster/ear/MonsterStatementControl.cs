using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatementControl : MonoBehaviour
{
    public Slider sld;
    public Slider sld_pla;
    public Animator aniMon;
    public Animator aniPla;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        aniMon.SetInteger("MonsterScore", (int)sld.value);
        aniMon.SetInteger("PlayerScore", (int)sld_pla.value);
        aniPla.SetInteger("MonsterScore", (int)sld.value);
        aniPla.SetInteger("PlayerScore", (int)sld_pla.value);
        int diff = (int)(sld.value - sld_pla.value);
        aniPla.SetInteger("diff", (diff >= 0) ? diff : 0);
    }
}
