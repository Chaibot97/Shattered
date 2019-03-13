using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour {

    public string promptForRequirement = "";

    public GameObject target=null;
    public GameObject requirement = null;
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
        float t = anim.GetNextAnimatorStateInfo(0).length;



        switch (interactOption)
        {
            case InteractOptions.Toggle:
                if(anim)anim.SetBool("open", !GetComponent<Animator>().GetBool("open"));
                break;
            case InteractOptions.Reveal:
                if (anim)anim.SetBool("check", true);
                if (objToReveal) StartCoroutine(Reveal());
                break;
        }


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
}
