using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TittleScreen : MonoBehaviour {

    public GameManager _GM;
    public Button resume;
    public Button start;

    void Start()
    {
        _GM = GameObject.Find("_GM").GetComponent<GameManager>();
        start.onClick.AddListener(StartGame);
        Debug.Log(_GM.sceneReached);
        if (_GM.sceneReached != 0)
        {
            resume.gameObject.SetActive(true);
        }
        else
        {
            resume.gameObject.SetActive(false);
        }
        resume.onClick.AddListener(ResumeGame); 
    }

    void StartGame()
    {
        VideoPlayer video = GetComponent<VideoPlayer>();
        video.Play();
        StartCoroutine(GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadSceneWithMovieHelper(1,6));
    }
    void ResumeGame()
    {
        StartCoroutine(GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadSceneWithFading(_GM.sceneReached));
    }
}
