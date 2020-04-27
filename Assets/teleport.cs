using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public GameObject empty1;
    public GameObject empty2;
    public GameObject player;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Mirror")
        {
            Debug.Log("go to inside");
            player.transform.position = empty2.transform.position;
        }
        else if (col.gameObject.name == "Mirror2")
        {
            Debug.Log("go back");
            player.transform.position = empty1.transform.position;
        }
    }


}
