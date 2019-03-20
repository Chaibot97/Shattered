using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv1Progress : MonoBehaviour
{
    [SerializeField] public bool diaryFound;
    [SerializeField] public bool note2Found;
    [SerializeField] public bool note3Found;
    [SerializeField] public bool newspaperFound;
    [SerializeField] public bool paintFound;
    [SerializeField] public bool safeFound;
    [SerializeField] public bool safeUnlocked;
    [SerializeField] public bool finished;
    [SerializeField] public bool changephoto;
    [SerializeField] public bool photoFound;

    public GameObject page3content;
    public GameObject page4content;
    public GameObject page5content;
    public GameObject page6content;
    public GameObject page7content;
    public GameObject page8content;
    public GameObject firePlace;
    public GameObject filled_water;
    public Camera SecondCamera;
    public Camera PrimaryCamera;
    public GameObject Photo;
    [HideInInspector] public int freeze;

    public SceneLoader sl;

    public UnityStandardAssets.Characters.FirstPerson.PlayerInteraction PInteract;
    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController Player;
    private bool diaryComplete;
    public bool DiaryComplete { get { return diaryComplete; } }
    private void Start()
    {
        freeze = 360;
    }
    private void Update()
    {
        if(freeze <= 360)
        {
            freeze++;
        }
        if (note2Found)
        {
            page3content.SetActive(true);
            page4content.SetActive(true);
        }
        if (note3Found)
        {
            page5content.SetActive(true);
            page6content.SetActive(true);
        }
        if (newspaperFound)
        {
            page8content.SetActive(true);
        }
        if (paintFound)
        {
            page7content.SetActive(true);
        }
        //if(!safeFound && note2Found && note3Found && newspaperFound && paintFound)
        //{
        //    firePlace.GetComponent<Collider>().enabled = true;
        //    diaryComplete = true;
        //}
        if (finished)
        {
            //sl.LoadScene("level_1");
            //StartCoroutine(sl.LoadSceneWithFading("Corridor1"));

        }

        if (changephoto)
        {
            //if (!PInteract.islooking)
            //{
            //    filled_water.GetComponent<Renderer>().material.shader = Shader.Find("Shader_highlight/0.TheFirstShader");
            //}
            if (PInteract.cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)) && !PInteract.islooking)
            {
                PInteract.cd = 0;
                if ((!PInteract.photo_changed) && photoFound)
                {
                    freeze = 0;
                }
                Player.enableInput = false;
                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.x = 65;
                rotationVector.y = 75;
                PrimaryCamera.transform.rotation = Quaternion.Euler(rotationVector);
                var rotationVector1 = transform.rotation.eulerAngles;
                rotationVector1.y = 180;
                PrimaryCamera.gameObject.transform.parent.rotation = Quaternion.Euler(rotationVector1);
                PrimaryCamera.gameObject.transform.parent.position = new Vector3(6.7f, 1.607f, 7.002f);
                foreach (Transform child in Photo.transform)
                {
                    if (child.gameObject.tag != "Untagged")
                    {
                        child.gameObject.tag = "Untagged";
                    }
                }
                Interactable i = filled_water.gameObject.GetComponent<Interactable>();
                if (!PInteract.photo_pickedup && PInteract.inventory.Contains(i.requirement))
                {
                    i.Interact();
                    PInteract.DestroyObj(PInteract.inventory.IndexOf(i.requirement));
                    Photo.gameObject.SetActive(true);
                    PInteract.photo_changed = true;
                }
                SecondCamera.gameObject.SetActive(true);
                PrimaryCamera.gameObject.SetActive(false);
                filled_water.GetComponent<Renderer>().material.shader = Shader.Find("Standard (Roughness setup)");
                PInteract.islooking = true;


            }
            if (freeze >= 360 && PInteract.cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)) && PInteract.islooking)
            {
                foreach (Transform child in Photo.transform)
                {
                    if (child.gameObject.name == "Photo_changed")
                    {
                        child.gameObject.tag = "Pickupable";
                    }
                }
                PInteract.cd = 0;
                SecondCamera.gameObject.SetActive(false);
                PrimaryCamera.gameObject.SetActive(true);
                filled_water.GetComponent<Renderer>().material.shader = Shader.Find("Standard (Roughness setup)");
                PInteract.islooking = false;
                Player.enableInput = true;

            }
            //changephoto = false;
        }
    }
}

