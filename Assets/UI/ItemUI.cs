using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour {

    private Text subtitle;
    private Image highlighter;
    private Image inspector;
    public GameObject book;

    public string objName="";
    public Item item;
    public bool isOn = false;
   
    // Use this for initialization
    void Start () {
        subtitle = GameObject.Find("Subtitle").GetComponent<Text>();
        highlighter = GameObject.Find("Highlighter").GetComponent<Image>();
        inspector = GameObject.Find("Inspector").GetComponent<Image>();

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
        if (!(objName==""))
        {
            subtitle.text = objName;
        
        }
        else 
        {
            subtitle.text = "";
        }
        highlighter.enabled = true;
        highlighter.transform.position = transform.position;
    }

    public void OnPointerExitDelegate()
    {
        subtitle.text = "";
        highlighter.enabled = false;
    }

    public void OnPointerUpDelegate()
    {
        if (objName == "")
            return;
        if (objName == "Diary")
        {
            if(book)book.SetActive(true);
            inspector.enabled = false;
        }
        else
        {
            if (book)book.SetActive(false);
            inspector.enabled = true;
            if(item)
                inspector.sprite = item.largeSprite;

        }
            
    }
}
