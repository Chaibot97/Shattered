using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TittleScreen : MonoBehaviour {

    public GameManager _GM;
    public Button resume;
    public Button start;
    public GameObject panel;
    public GameObject queryBox;
    void Start()
    {
        _GM = GameObject.Find("_GM").GetComponent<GameManager>();
        Debug.Log(_GM.sceneReached);
        if (_GM.sceneReached != 0)
        {
            resume.gameObject.SetActive(true);
            start.onClick.AddListener(QueryStartGame);
        }
        else
        {
            resume.gameObject.SetActive(false);
            start.onClick.AddListener(StartGame);
        }
        resume.onClick.AddListener(ResumeGame); 
    }
    void QueryStartGame()
    {
        panel.SetActive(false);
        queryBox.SetActive(true);
    }
    public void StartGame()
    {
        VideoPlayer video = GetComponent<VideoPlayer>();
        video.Play();
        StartCoroutine(GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadSceneWithMovieHelper(1,4));
    }
    void ResumeGame()
    {
        StartCoroutine(GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadSceneWithFading(_GM.sceneReached));
    }
}
