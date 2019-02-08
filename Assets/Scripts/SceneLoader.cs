using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {

    void Start(){   
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
     }
    public void LoadScene(String level)
    {
        Time.timeScale=1f;
        SceneManager.LoadScene(level);     
    }

    public void ExitGame()
    {

        //save game!!!

    #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();

    #endif
    }
}
