using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewESCMenu : MonoBehaviour
{
    public GameObject Menu;
    public bool Paused = false;

    UnityStandardAssets.Characters.FirstPerson.Interaction player;

    void Start()
    {
        Menu.gameObject.SetActive(false);
        player = GameObject.FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.Interaction>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused == true)
            {
                Time.timeScale = 1.0f;
                Menu.gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Paused = false;

            }
            else if (!player.gamePaused)
            {
                Time.timeScale = 0.0f;
                Menu.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Paused = true;
            }
        }
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        Menu.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Paused = false;
    }
}
