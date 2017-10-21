using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public class BoyBeHurtedControl  : PlayableBehaviour
{
    public GameObject hitboy;

    //开始播放的时候找到物体
    public override void OnGraphStart(Playable playable)
    {
        hitboy = GameObject.Find("hitBoy");
    }

    //使物体消失
    public override void  OnGraphStop(Playable playable)
    {
        hitboy.SetActive(false);
    }




}
