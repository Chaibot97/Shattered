using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour {
    public Filter filter;
    public MirrorReflection m1;
    public MirrorReflection m2;

    void Start () {
		
	}
	
	public void Blink()
    {
        filter.blink();
    }
    public void ChangeMirror()
    {
        StartCoroutine(ChangeMirrorHelper());
    }
    public IEnumerator ChangeMirrorHelper()
    {
        if (m2) m2.enabled=true;
        yield return new WaitForSeconds(0.2f);
        if (m1) m1.enabled = false;
    }
}
