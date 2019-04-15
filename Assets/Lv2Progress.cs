using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv2Progress : MonoBehaviour {
    public GameObject oldBunny;
    public GameObject[] bunnys;
    public GameObject Lights;
    private AudioSource turnoff;
    private AudioSource laugh;
    public Filter filter;

    private void Start()
    {
        turnoff = GetComponents<AudioSource>()[1];
        laugh = GetComponents<AudioSource>()[2];
    }
    public void BunnyHop()
    {
        StartCoroutine(SwitchBunnys());
    }

    public void TurnOff()
    {
        StartCoroutine(TurnOffHelper());
    }

    IEnumerator SwitchBunnys()
    {
        foreach (GameObject b in bunnys)
        {
            filter.blink();
            yield return new WaitForSeconds(0.2f);
            oldBunny.SetActive(false);
            b.SetActive(true);
            oldBunny = b;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator TurnOffHelper()
    {
        yield return new WaitForSeconds(1f);
        turnoff.Play();
        Lights.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        laugh.Play();
        yield break;

    }
}
