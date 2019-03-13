using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {
    //public Filter filter;
    public Canvas canvas;
    void Start(){   
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
     }
    public void LoadScene(String level)
    {
        Time.timeScale=1f;
        SceneManager.LoadScene(level);     
    }
    public IEnumerator LoadSceneWithFading(String level)
    {
        float t = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(level);
    }

    public IEnumerator LoadNextSceneWithFading()
    {
        float t = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator LoadSceneWithMovieHelper(String level,float time)
    {

        yield return new WaitForSeconds(time);
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
