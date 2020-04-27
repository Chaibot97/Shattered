using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant : MonoBehaviour
{
    public GameObject plant1;
    public GameObject plant2;
    public GameObject ladder;
    public GameObject anim1;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "plantShadow")
        {
            Debug.Log("find plant!");
            anim1.SetActive(true);
            Destroy(gameObject);
            Destroy(ladder);
            Destroy(plant2);
            plant1.SetActive(true);
        }
    }
}
