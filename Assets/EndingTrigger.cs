using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndingTrigger : MonoBehaviour {
    public Camera EndingCamera;
    public float delay;
    public SceneLoader sl;
    public Text prompt;
    public void End()
    {
        EndingCamera.enabled = true;
        StartCoroutine(EndRoutine());
    }

    public IEnumerator EndRoutine()
    {
        EndingCamera.GetComponent<Animator>().SetTrigger("end");
        yield return new WaitForSeconds(delay);
        prompt.text = "Press any key to continue...";
        while (!Input.anyKey)
        {
            yield return new WaitForEndOfFrame();
        }
        if (sl._GM)
            sl._GM.sceneReached = 0;
        StartCoroutine(sl.LoadNextSceneWithFading());
    }
}
