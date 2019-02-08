using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Transition : MonoBehaviour {
    RawImage mask;
    Canvas uis;
    bool started=false;
    float time;
    public float duration;
    Color c = Color.black;
    private void Start()
    {
        mask = GetComponent<RawImage>();
        uis = GetComponent<Canvas>();


    }
    public void putMask(){

        mask.enabled = true;
        started = true;
        time = duration;

    }
    private void Update()
    {
        if(started && time>0){
            time--;
            c.a = time / duration;
            mask.color=c;
            //Debug.Log(time);
        }
        if(time<=0){
            mask.enabled = false;
        }
    }

}
