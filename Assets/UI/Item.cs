using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour {

    private Text subtitle;

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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnterDelegate()
    {
        if (!this.name.Equals("Empty"))
        {
            subtitle.text = this.name;
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
}
