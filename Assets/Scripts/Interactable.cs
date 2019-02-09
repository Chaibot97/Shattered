using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public bool once = false;
	public void interact(){
		GetComponent<Animator>().SetBool("open",!GetComponent<Animator>().GetBool("open"));
        if (once)
        {
            gameObject.tag = "Untagged";
        }
	}
}
