using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public int sceneReached = 0;
	// Use this for initialization
	void Start () {
        //if (GameObject.Find("_GM") && GameObject.Find("_GM") != this)
        //    DestroyImmediate(this);
        //else
            DontDestroyOnLoad(this);
    }
	
}
