using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenu : MonoBehaviour {

     
     public GameObject Menu;
     bool Paused = false;
 
     void Start(){
         Menu.gameObject.SetActive (false);
     }
 
     void Update () {
         if (Input.GetKeyDown (KeyCode.Escape)) {
             if(Paused == true){
				Time.timeScale = 1.0f;
				Menu.gameObject.SetActive (false);
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				Paused = false;
             } else {
				Time.timeScale = 0.0f;
				Menu.gameObject.SetActive (true);
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				Paused = true;
             }
         }
     }
     public void Resume(){
		Time.timeScale = 1.0f;
		Menu.gameObject.SetActive (false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Paused = false;
     }
   
}
