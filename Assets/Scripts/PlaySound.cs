using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    AudioSource audioData1;
    AudioSource audioData2;
    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        audioData1 = audios[0];
        audioData2 = audios[1];
    }
    public void Play0()
    {
        audioData1.Play();
    }
    public void Play1()
    {
        audioData2.Play();
    }
}
