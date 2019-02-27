using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TittleScreen : MonoBehaviour {

    public Button start;

    void Start()
    {
        start.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        VideoPlayer video = GetComponent<VideoPlayer>();
        video.Play();
        StartCoroutine(GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadSceneWithMovieHelper("FirstLevel",6));
    }
}
