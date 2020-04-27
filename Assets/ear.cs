using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ear : MonoBehaviour
{
    public GameObject bunny;
    public GameObject hiddenone;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "NoEar")
        {
            Debug.Log("Find ear!");
            hiddenone.SetActive(true);
            Destroy(gameObject);
            Destroy(bunny);
        }
    }
}
