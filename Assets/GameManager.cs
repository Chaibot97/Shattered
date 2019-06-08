using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public int sceneReached=0;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        sceneReached=PlayerPrefs.GetInt("sceneReached", 0);
    }
	
}
