using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv1Progress : MonoBehaviour {
    [SerializeField] public bool diaryFound;
    [SerializeField] public bool note2Found;
    [SerializeField] public bool note3Found;
    [SerializeField] public bool newspaperFound;
    [SerializeField] public bool photoFound;
    [SerializeField] public bool safeUnlocked;

    public GameObject page3content;
    public GameObject page4content;
    public GameObject page5content;
    public GameObject page6content;
    public GameObject page7content;
    public GameObject page8content;

    private void Update()
    {
        if (note2Found)
        {
            page3content.SetActive(true);
            page4content.SetActive(true);
        }
    }
}
