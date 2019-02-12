using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv1Progress : MonoBehaviour {
    [SerializeField] public bool diaryFound;
    [SerializeField] public bool note2Found;
    [SerializeField] public bool note3Found;
    [SerializeField] public bool newspaperFound;
    [SerializeField] public bool photoWashed;
    [SerializeField] public bool safeFound;
    [SerializeField] public bool safeUnlocked;

   
    public GameObject page3content;
    public GameObject page4content;
    public GameObject page5content;
    public GameObject page6content;
    public GameObject page7content;
    public GameObject page8content;
    public GameObject firePlace;


    private bool diaryComplete;
    public bool DiaryComplete { get { return diaryComplete; } }

    private void Update()
    {
        if (note2Found)
        {
            page3content.SetActive(true);
            page4content.SetActive(true);
        }
        if (note3Found)
        {
            page5content.SetActive(true);
            page6content.SetActive(true);
        }
        if (newspaperFound)
        {
            page8content.SetActive(true);
        }
        if (photoWashed)
        {
            page7content.SetActive(true);
        }
        if(!safeFound && note2Found && note3Found && newspaperFound && photoWashed)
        {
            firePlace.GetComponent<Collider>().enabled = true;
            diaryComplete = true;
        }
        if (safeFound)
        {
            firePlace.GetComponent<Collider>().enabled = false;
        }
    }
}
