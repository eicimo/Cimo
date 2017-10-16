using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatementControl : MonoBehaviour {
    public Slider sld;
    public Animator ani;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ani.SetInteger("MonsterScore", (int)sld.value);
	}
}
