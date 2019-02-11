using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public string promptForRequirement = "";

    public GameObject target=null;
    public GameObject requirement = null;
    public enum InteractOptions
    {
        Toggle,
        Reveal
    }
    public InteractOptions interactOption;
    public GameObject ObjToReveal;

    public bool once = false;

    private void Start()
    {
        if (!target)
        {
            target = gameObject;
        }
    }

    public void Interact(){
        Animator anim = target.GetComponent<Animator>();

        switch (interactOption)
        {
            case InteractOptions.Toggle:
                if(anim)anim.SetBool("open", !GetComponent<Animator>().GetBool("open"));
                break;
            case InteractOptions.Reveal:
                if(ObjToReveal)ObjToReveal.SetActive(true);
                if (anim)anim.SetBool("check", true);
                break;
        }
      


		
        if (once)
        {
            if (gameObject.name != "Sink")
            {

                gameObject.tag = "Untagged";
                Renderer[] rend = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in rend)
                {
                    r.material.shader = Shader.Find("Standard (Roughness setup)");
                }
                gameObject.GetComponent<Collider>().enabled = false;
            }
            
        }
	}
}
