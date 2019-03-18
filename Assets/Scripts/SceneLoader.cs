using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {
    //public Filter filter;
    public GameManager _GM;
    public Canvas canvas;
    void Start(){   
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject g = GameObject.Find("_GM");
        if (!g)
        {
            //SceneManager.LoadScene(0);
        }
        else
        {
            _GM = g.GetComponent<GameManager>();
        }
    }
    public void LoadScene(int level)
    {
        Time.timeScale=1f;
        if (_GM && level > _GM.sceneReached)
        {
            _GM.sceneReached = level;
        }
        SceneManager.LoadScene(level);     
    }
    public IEnumerator LoadSceneWithFading(int level)
    {
        float t = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(t);
        LoadScene(level);
    }

    public IEnumerator LoadNextSceneWithFading()
    {
        float t = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(t);
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator LoadSceneWithMovieHelper(int level,float time)
    {

        yield return new WaitForSeconds(time);
        LoadScene(level);
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
