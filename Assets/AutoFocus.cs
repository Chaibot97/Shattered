using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoFocus : MonoBehaviour
{
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        
    }
    void OnEnable()
    {
        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

}
