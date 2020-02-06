using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform target;
    public GameObject hide;
    public GameObject show;
    public void Teleport()
    {
        StartCoroutine(TeleportHelper());
        gameObject.GetComponent<AudioSource>().Play();
    }
    public IEnumerator TeleportHelper()
    {
        float t = GameObject.FindObjectOfType<Fading>().BeginFade(1);
        yield return new WaitForSeconds(t/2);
        GameObject.FindGameObjectWithTag("Player").transform.position = target.position;
        GameObject.FindObjectOfType<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(0.2f);
        if (show) show.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        if (hide) hide.SetActive(false);
    }
}
