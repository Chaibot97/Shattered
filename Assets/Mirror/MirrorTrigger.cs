using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MirrorTrigger : MonoBehaviour {

    public GameObject obj;
    public Filter filter;
    private float angle;
    private bool done=false;
    public float Angle { get { return angle; } }


    // Use this for initialization
    void Start()
    {
        if (obj)
        {
            Vector3 direction = obj.transform.position - transform.position;
            angle = Vector3.SignedAngle(direction, transform.up, Vector3.up);
            Debug.Log(angle);
            obj.GetComponent<Collider>().enabled = false;
            //obj.SetActive(false);
        }
    }

    public void reveal(float ang) {

        if (done)
            return;
        int diff = (int)Mathf.Min(Mathf.Abs(ang + angle), 30);
        GetComponent<MirrorReflection>().m_TextureSize = Mathf.Max((int)120 * (10 - diff/3), 128);
        Debug.Log(Mathf.Max((int)120 * (10 - diff/2), 128));
        if(!done&& Mathf.Max((int)120 * (10 - diff/2), 128)>1000)
        {
            //obj.SetActive(true);
            obj.layer = 0;
            obj.GetComponent<Collider>().enabled = true;
            Debug.Log("reveal!");
            filter.blink();
            GetComponent<MirrorReflection>().m_TextureSize = 1024;
            done = true;
        }
    }
}
