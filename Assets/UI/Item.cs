using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour {

    private Text subtitle;
    public GameObject book;
    public string objName="Empty";
    public bool isOn = false;
   
    // Use this for initialization
    void Start () {
        subtitle = GameObject.Find("Subtitle").GetComponent<Text>();
        
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate(); });
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExitDelegate(); });
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnPointerUpDelegate(); });
        trigger.triggers.Add(entry);
        
    }
	

    public void OnPointerEnterDelegate()
    {
        if (!objName.Equals("Empty"))
        {
            subtitle.text = objName;
        }
        else 
        {
            subtitle.text = "";
        }
    }

    public void OnPointerExitDelegate()
    {
        subtitle.text = "";
    }

    public void OnPointerUpDelegate()
    {
        if (objName == "diary")
        {
            if(book)book.SetActive(true);
        }
        else
        {
            if (book)book.SetActive(false);
        }
            
    }
}
