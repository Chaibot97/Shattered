using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInteraction : MonoBehaviour {

    //[SerializeField] AudioClip takeItem;
    //[SerializeField] AudioClip teleport;
    private Transform target;
    private bool holding;
    private Collider itemHolding; 
    private int cd;

    private void Start()
    {
        holding = false;
        cd = 10;
        target = Camera.main.transform;
    }

    private void Update()
    {
   

        if (itemHolding && holding)
        {
            HoldItem(itemHolding);
        }
     
    }


    void OnTriggerEnter(Collider col)
    {
       
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Contains("Pickupable"))
        {
            float angle = Vector3.Angle(col.transform.position-target.transform.position, target.transform.forward);
            if (angle <= 80 * 0.5f)
            {
                if (cd >= 30 && Input.GetMouseButtonUp(0))
                {
                    cd = 0;
                    holding = !holding;
                    if (holding)
                    {
                        itemHolding = col;
                        itemHolding.GetComponent<CapsuleCollider>().isTrigger = true;

                    }
                    else
                    {
                        itemHolding.GetComponent<CapsuleCollider>().isTrigger = false;
                        itemHolding.transform.forward = Vector3.Scale(itemHolding.transform.forward, new Vector3(1, 0, 1));
                        itemHolding = null;
                    }
                    Debug.Log(holding);

                }
                cd++;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        
    }
    private void HoldItem(Collider col)
    {
        Vector3 offset = target.transform.forward;
        //offsetScale = (float)Mathf.Cos(Vector3.Angle(target.transform.forward, transform.forward) * Mathf.PI / 180);
        col.transform.position= target.transform.position+offset*0.8f;
        //col.GetComponent<Rigidbody>().MovePosition(target.transform.position + offset);
        //col.transform.LookAt(target.transform.position+ target.transform.up*(col.transform.position.y- target.transform.position.y));
        col.transform.LookAt(target.transform.position);
    }

}
