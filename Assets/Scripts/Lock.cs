using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lock : MonoBehaviour {
    public string c_input="";
    public Text text;
    public string combination;
    public bool unlocked = false;


    void Update()
    {
        if (unlocked)
            return;
        string tmp = "";
        for(int i = 0;i < 8; i++) 
        {
            if(i< c_input.Length)
                tmp+=" "+ c_input[i];
            else
                tmp += " _";
        }
        text.text = tmp;
        
    }

}
