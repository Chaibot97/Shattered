using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public enum InteractOptions
    {
        Toggle,
        Reveal
    }
    public InteractOptions interactOption;
    public GameObject ObjToReveal;

    public bool once = false;
    public GameObject requirement = null;
    

	public void interact(){
        switch (interactOption)
        {
            case InteractOptions.Toggle:
                GetComponent<Animator>().SetBool("open", !GetComponent<Animator>().GetBool("open"));
                break;
            case InteractOptions.Reveal:
                if(ObjToReveal)ObjToReveal.SetActive(true);
                break;
        }
      


		
        if (once)
        {
            gameObject.tag = "Untagged";
        }
	}
}
