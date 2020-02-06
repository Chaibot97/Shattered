using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour {

    public Sprite smallSprite;
    public Sprite largeSprite;

    public GameObject[] TriggerObjs;
    public string[] TriggerFunctions;

    // Use this for initialization
    void Start () {
        
    }

    public void Trigger()
    {
        if (TriggerObjs.Length!=0 && TriggerObjs.Length==TriggerFunctions.Length)
        {
            Debug.Log("a");
            for(int i=0;i< TriggerObjs.Length;i++)
            {
                TriggerObjs[i].BroadcastMessage(TriggerFunctions[i]);
            }
        }
    }
}
