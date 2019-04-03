using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDoor : MonoBehaviour {

    public GameObject mainDoor;

    public void SwitchDoor()
    {
        StartCoroutine(SwitchDoorRoutine());
    }

    public IEnumerator SwitchDoorRoutine()
    {
        GetComponent<Animator>().SetBool("check", false);
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
        mainDoor.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
