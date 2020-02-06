using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Filter : MonoBehaviour {
    public Animator filterWhite;

    void start()
    {
        filterWhite = this.GetComponent<Animator>();
    }
    public void blink()
    {
        filterWhite.Play("Filter",0,0);
    }
}
