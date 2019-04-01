using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour {
    public Filter filter;


    void Start () {
		
	}
	
	public void Blink()
    {
        filter.blink();
    }
}
