using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LockButton : MonoBehaviour {

    public Lock l;

	// Use this for initialization
	void Awake() {
        Button b = GetComponent<Button>();
        if (name == "OK")
        {
            b.onClick.AddListener(OkClicker);
        }
        else if (name == "CLR")
        {
            b.onClick.AddListener(CLRClicker);
        }
        else
        {
            b.onClick.AddListener(NumClicker);
        }


    }

    private void NumClicker()
    {
        if (l.c_input.Length <= 8)
            l.c_input += name;
    }
    private void OkClicker()
    {
        if (l.c_input == l.combination)
        {
            l.unlocked = true;
        }
    }
    private void CLRClicker()
    {
        l.c_input = "";
    }


}
