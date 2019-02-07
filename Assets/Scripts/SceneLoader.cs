using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {

    public void LoadScene(String level)
    {
        if (level == "MenuState")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
