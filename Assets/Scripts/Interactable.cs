using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour {

    public string promptForRequirement = "";
    public string promptAfter = "";

    public GameObject target=null;
    public GameObject requirement = null;
    public bool destrayRequired = true;
    public GameObject objToHide = null;
    private Lv1Progress lv1_p;
    

    public enum InteractOptions
    {
        Toggle,
        Reveal
    }
    public InteractOptions interactOption;
    public GameObject objToReveal;


    public bool once = false;
    public bool disableCollider = true;
    Animator anim;
    public float delay;

    public GameObject[] TriggerObjs;
    public string[] TriggerFunctions;

    private void Start()
    {

        if (!target)
        {
            target = gameObject;
        }
        if (SceneManager.GetActiveScene().name.Equals("FirstLevel"))
        {
            lv1_p = GetComponent<Lv1Progress>();
        }
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(delay);
        objToHide.SetActive(false);
    }
    private IEnumerator Reveal()
    {
        yield return new WaitForSeconds(delay);
        objToReveal.SetActive(true);
    }

    public void Interact(){
        anim = target.GetComponent<Animator>();



        switch (interactOption)
        {
            case InteractOptions.Toggle:
                if(anim)anim.SetBool("open", !GetComponent<Animator>().GetBool("open"));
                break;
            case InteractOptions.Reveal:
                if (anim)anim.SetBool("check", true);
                //if (anim) anim.SetBool("open", true);
                if (objToReveal) StartCoroutine(Reveal());
                break;
        }
        if (promptAfter.Length != 0) 
        {
            GameObject.FindGameObjectWithTag("Player").BroadcastMessage("Prompt",  promptAfter);
        }
        Trigger();


        if (objToHide)
        {
            StartCoroutine(Hide());
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
                if (disableCollider)
                {
                   gameObject.GetComponent<Collider>().enabled = false;

                }
            }
            
        }
	}

    public void Trigger()
    {
        if (TriggerObjs.Length != 0 && TriggerObjs.Length == TriggerFunctions.Length)
        {
            for (int i = 0; i < TriggerObjs.Length; i++)
            {
                TriggerObjs[i].BroadcastMessage(TriggerFunctions[i]);
            }
        }
    }

    public void EnableInteraction()
    {
        gameObject.tag = "Interactable";
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
