using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "bunnyUP")
        {
            Debug.Log("You find me! Game end!");
        }
    }
}
