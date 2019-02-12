using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class PlayerInteraction : MonoBehaviour {

        public GameObject filled_water;
        public Camera SecondCamera;
        public Camera PrimaryCamera;
        public GameObject Photo;
        private readonly int pickUpFOV = 60;
        //[SerializeField] AudioClip takeItem;
        //[SerializeField] AudioClip teleport;
        private Transform target;
        private bool holding;

        private Collider itemChecking;
        private Shader shader1;
        private Shader shader2;
        private Renderer[] rend = { };
        private AudioSource putin;
        private AudioSource findpickup;
        private bool soundplayed;
        private bool alreadyfind;
        private bool filled;
        private bool islooking;
        private bool photo_changed;
        private bool photo_pickedup;

        private List<GameObject> inventory;
        private bool inSight = false;

        private bool checkingMirror=false;
        private bool checkingInventory = false;
        private bool checkingSafe;
        public GameObject book;
        [SerializeField] public Canvas inventoryUI;
        private Animator inventoryAnim;
        [SerializeField] public Canvas inspectorUI;
        private Animator inspectorAnim;
        [SerializeField] public Sprite UIMask;

        [SerializeField] public Text prompt;
        public GameObject lockUI;
        //LayoutGroup inventoryUI;
        private int cd;
        private int cd_sound;
        private int wait;
        
        private int playerLayerMask=1<<9;
        private Lv1Progress lv1_p;
        private void Start()
        {
            holding = false;
            soundplayed = false;
            alreadyfind = false;
            filled = false;
            photo_changed = false;
            islooking = false;
            photo_pickedup = false;
            cd = 30;
            cd_sound = 180;
            wait = 180;
            target = Camera.main.transform;
            shader1 = Shader.Find("Standard (Roughness setup)");
            shader2 = Shader.Find("Shader_highlight/0.TheFirstShader");
            inventory = new List<GameObject>(5);
            for (int i = 0; i < 5; i++) 
                inventory.Add(new GameObject());

            inventoryAnim = inventoryUI.GetComponent<Animator>();
            inspectorAnim = inspectorUI.GetComponent<Animator>();

            AudioSource[] audios = GetComponents<AudioSource>();
            putin = audios[2];
            findpickup = audios[1];

            if (SceneManager.GetActiveScene().name.Equals("FirstLevel"))
            {
                lv1_p = GetComponent<Lv1Progress>();
            }
            book.SetActive(false);
        }

      
        private void LateUpdate()
        {

            if(cd<30)
                cd++;
            if (cd_sound < 180)
                cd_sound++;
            if (wait < 180)
                wait++;
            if (checkingSafe)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    checkingSafe = false;
                    lockUI.SetActive(false);
                    PlayerEnable(true);
                }
                if (lockUI.GetComponent<Lock>().unlocked)
                {
                    checkingSafe = false;
                    lockUI.SetActive(false);
                    PlayerEnable(true);
                    itemChecking.GetComponent<Collider>().enabled = false;
                    itemChecking.GetComponent<Animator>().SetBool("lock", true);
                }
            }
            else if (!checkingMirror)
            {
                if (!islooking)
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        ToggleInventory();
                    }
                   
                }
            }
            else
            {

                target.transform.LookAt(itemChecking.gameObject.transform);
                Vector3 direction = transform.position- itemChecking.transform.position;
                float angle = Vector3.SignedAngle(new Vector3(direction.x, 0, direction.z), itemChecking.transform.up, Vector3.up);
                if (itemChecking.GetComponent<MirrorTrigger>().reveal(angle))
                {
                    StartCoroutine(MirrorDone(itemChecking,1));
                }
                if (Input.GetKey(KeyCode.D)&& angle<35)
                {
                    GetComponent<Rigidbody>().AddForce(transform.right * 1f, ForceMode.Impulse);
                }
                if (Input.GetKey(KeyCode.A) && angle >- 35)
                {
                    GetComponent<Rigidbody>().AddForce(transform.right * -1f, ForceMode.Impulse);
                }
                
            }


            //if (Input.GetKeyUp(KeyCode.O))
            //{
            //    int i = 0;
            //    foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
            //    {
            //        if (lp.tag != "ItemUI")
            //            continue;
            //        Toggle checker = lp.GetComponentInChildren<Toggle>();
            //        if (checker.isOn)
            //        {
            //            lp.gameObject.name = "Empty";
            //            lp.GetComponentsInChildren<Image>()[1].sprite = UIMask;
            //            checker.isOn = false;

            //            GameObject item = inventory[i];
            //            inventory[i] = null;
            //            item.transform.position = transform.position + transform.forward * 3;
            //            item.SetActive(true);
            //            break;

            //        }
            //        i++;
            //    }
            //}
        }

        void OnTriggerEnter(Collider col)
        {

        }

        void OnTriggerStay(Collider col)
        {
            
            if (checkingInventory|| checkingSafe)
                return;
            inSight = false;
            
            if (itemChecking && col != itemChecking) return;

            if (col.gameObject.tag.Equals("Mirror"))
            {
                itemChecking = col;
                if (!checkingMirror)
                {
                    Vector3 direction = col.transform.position - target.transform.position;
                    float angle = Vector3.Angle(direction, target.transform.forward);
                    prompt.text="";
                    if (angle <= pickUpFOV * 0.8f)
                    {
                        RaycastHit hit;
                        Debug.DrawRay(col.transform.position , col.transform.up, Color.red);
                        if (Physics.Raycast(col.transform.position, col.transform.up, out hit, 1))
                        {
                            //Debug.Log(hit.collider.name);
                            if (hit.collider.tag.Contains("Player"))
                            {
                                inSight = true;
                                prompt.text = "Press E to inspect.";
                                if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
                                {
                                    cd = 0;
                                    itemChecking = col;
                                    checkingMirror = true;
                                    Vector3 tmp = col.gameObject.transform.position + col.gameObject.transform.up * 1.3f;
                                    transform.position = ( new Vector3(tmp.x,transform.position.y,tmp.z));
                                    target.transform.LookAt(col.gameObject.transform);
                                    GetComponent<RigidbodyFirstPersonController>().enableInput = false;

                                    prompt.text = "Press A/D to change angle. Press E to quit.";
                                    return;

                                }
                            }
                        }
                    }
                    else
                    {
                        prompt.text="";
                    }
                }
                else
                {
                    if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
                    {
                        cd = 0;
                        prompt.text = "";
                        checkingMirror = false;
                        //GetComponent<RigidbodyFirstPersonController>().enabled = true;

                        GetComponent<RigidbodyFirstPersonController>().enableInput = true;

                    }
                }
            }else if (col.gameObject.tag.Equals("Interactable")|| col.gameObject.tag.Equals("Safe") )
            {
                Vector3 direction = col.transform.position.normalized - target.transform.position.normalized;
                float angle = Vector3.Angle(direction, target.transform.forward);
                // Debug.Log(angle);

                itemChecking = col;
                
                RaycastHit hit;
                if (Physics.Raycast(target.transform.position, target.transform.forward, out hit,playerLayerMask, 5))
                {
                    if(hit.collider.Equals(col)){
                        if (itemChecking.name.Equals("Sink") && filled)
                        {
                            inSight = true;
                            
                            rend = itemChecking.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rend)
                            {
                                r.material.shader = shader1;
                            }
                            if(wait >= 180)
                            {
                                if (!islooking)
                                {
                                    filled_water.GetComponent<Renderer>().material.shader = shader2;
                                }
                                if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)) && !islooking)
                                {
                                    cd = 0;
                                    GetComponent<RigidbodyFirstPersonController>().enableInput = false;
                                    var rotationVector = transform.rotation.eulerAngles;
                                    rotationVector.x = 45;
                                    target.transform.rotation = Quaternion.Euler(rotationVector);
                                    var rotationVector1 = transform.rotation.eulerAngles;
                                    rotationVector1.y = 180;
                                    target.gameObject.transform.parent.rotation = Quaternion.Euler(rotationVector1);
                                    target.gameObject.transform.parent.position = new Vector3(6.7f, 1.407f, 7.002f);
                                    Interactable i = filled_water.gameObject.GetComponent<Interactable>();
                                    if (!photo_pickedup&& inventory.Contains(i.requirement))
                                    {
                                        i.Interact();
                                        DestroyObj(inventory.IndexOf(i.requirement));
                                        Photo.gameObject.SetActive(true);
                                        photo_changed = true;
                                    }
                                    SecondCamera.gameObject.SetActive(true);
                                    PrimaryCamera.gameObject.SetActive(false);
                                    filled_water.GetComponent<Renderer>().material.shader = shader1;
                                    islooking = true;
                                    

                                }
                                if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)) && islooking)
                                {
                                    cd = 0;
                                    SecondCamera.gameObject.SetActive(false);
                                    PrimaryCamera.gameObject.SetActive(true);
                                    filled_water.GetComponent<Renderer>().material.shader = shader1;
                                    islooking = false;
                                    GetComponent<RigidbodyFirstPersonController>().enableInput = true;

                                }
                            }
                            
                        }
                        else
                        {
                            inSight = true;
                            rend = col.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rend)
                            {
                                r.material.shader = shader2;
                            }
                            if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)))
                            {
                                cd = 0;
                                if (col.gameObject.tag.Equals("Safe"))
                                {
                                    lockUI.SetActive(true);
                                    PlayerEnable(false);
                                    checkingSafe = true;
                                }
                                else
                                {

                                    Interactable i = col.gameObject.GetComponent<Interactable>();
                                    if (!i.requirement)
                                    {
                                        i.Interact();
                                        if (itemChecking.gameObject.name.Equals("Chest"))
                                        {
                                            itemChecking = null;
                                        }
                                    }
                                    else if (inventory.Contains(i.requirement))
                                    {
                                        if (col.gameObject.name.Equals("Sink"))
                                        {
                                            Debug.Log("filled");
                                            wait = 0;
                                            filled = true;
                                        }
                                        DestroyObj(inventory.IndexOf(i.requirement));
                                        i.Interact();
                                        i.requirement = null;
                                    }
                                    else
                                    {

                                        StartCoroutine(ShowPrompt(i.promptForRequirement, 2));

                                    }
                                }
                            }
                        }
                        
                    }
                }
            
            }
            else if (col.gameObject.tag.Equals("Pickupable")|| col.gameObject.tag.Equals("Note"))
            {
                Vector3 direction = col.GetComponent<Renderer>().bounds.center - target.transform.position;
                float angle = Vector3.Angle(direction, target.transform.forward);
                itemChecking = col;
                if (angle <= pickUpFOV * 0.5f)
                {
                    RaycastHit hit;
                    Debug.DrawRay(target.transform.position, direction.normalized*5, Color.red);
                    if (Physics.Raycast(target.transform.position, direction.normalized , out hit,playerLayerMask, 5))
                    {
                        //Debug.Log(hit.collider.name);
                        if (hit.collider.Equals(col)){
                            inSight = true;
                            rend = col.GetComponentsInChildren<Renderer>();
                            if (!soundplayed && cd_sound == 180 && !alreadyfind)
                            {
                                findpickup.Play();
                                cd_sound = 0;
                                soundplayed = true;
                            }
                            alreadyfind = true;
                            foreach (Renderer r in rend)
                            {
                                r.material.shader = shader2;
                            }
                            if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)))
                            {
                                cd = 0;
                                if (col.gameObject.tag.Equals("Pickupable") && !col.gameObject.name.Equals("Photo_changed"))
                                {
                                    int i = 0;
                                    foreach (Image img in inventoryUI.GetComponentsInChildren<Image>())
                                    {
                                        if (img.tag != "ItemUI")
                                            continue;
                                        Item item = img.GetComponentInChildren<Item>();
                                        if (!item.isOn)
                                        {
                                            item.objName = col.gameObject.name;
                                            if (col.gameObject.GetComponent<Image>())
                                                img.sprite = col.gameObject.GetComponent<Image>().sprite;
                                            item.isOn = true;
                                            Debug.Log(i);
                                            inventory[i] = col.gameObject;
                                            col.gameObject.SetActive(false);
                                            inSight = false;
                                            StartCoroutine(ShowPrompt(item.objName + " found", 2));
                                            itemChecking = null;
                                            putin.Play();
                                            if (item.objName == "Diary")
                                            {
                                                lv1_p.diaryFound = true;
                                            }
                                            //    if (col.gameObject.transform.parent.gameObject.name.Equals("photo 1"))
                                            //{
                                            //    photo_pickedup = true;
                                            //    col.gameObject.transform.parent.gameObject.SetActive(false);
                                            //}
                                            break;

                                        }
                                        i++;
                                    }
                                }
                                else if (col.gameObject.tag.Equals("Note"))
                                {
                                    if (!lv1_p.diaryFound)
                                    {
                                        StartCoroutine(ShowPrompt("A paper scrap, I should something to hold it."));
                                    }
                                    else
                                    {
                                        if (col.gameObject.name == "Note2")
                                            lv1_p.note2Found = true;
                                        else if (col.gameObject.name == "Note3")
                                            lv1_p.note3Found = true;
                                        else if (col.gameObject.name == "Newspaper")
                                            lv1_p.newspaperFound = true;
                                        col.gameObject.SetActive(false);
                                        StartCoroutine(ShowPrompt("A paper scrap, added it to the diary."));

                                    }

                                }
                                else if (col.gameObject.name.Equals("Photo_changed"))
                                {
                                    if (!lv1_p.diaryFound)
                                    {
                                        StartCoroutine(ShowPrompt("A photo, I should something to hold it."));
                                    }
                                    else
                                    {
                                        photo_pickedup = true;
                                        lv1_p.photoFound = true;
                                        col.gameObject.transform.parent.gameObject.SetActive(false);
                                        StartCoroutine(ShowPrompt("A paper scrap, added it to the diary."));

                                    }

                                }
                            }
                        }
                    }
                }



            }
            if (!inSight)
            {
                filled_water.GetComponent<Renderer>().material.shader = shader1;
                foreach (Renderer r in rend)
                {
                    r.material.shader = shader1;
                    soundplayed = false;
                    alreadyfind = false; 
                }
                if (col.gameObject.tag.Equals("Pickupable") || col.gameObject.tag.Equals("Interactable") || col.gameObject.tag.Equals("Safe"))
                {
                    
                    itemChecking = null;
                }
                if (col.gameObject.tag.Equals("Mirror"))
                {
                    prompt.text="";
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            filled_water.GetComponent<Renderer>().material.shader = shader1;
            foreach (Renderer r in rend)
            {
                r.material.shader = shader1;
                soundplayed = false;
                alreadyfind = false;
            }
            if (col.gameObject.tag.Equals("Pickupable") || col.gameObject.tag.Equals("Interactable")|| col.gameObject.tag.Equals("Safe"))
            {    

                inSight = false;
                itemChecking = null;
            }
            if (col.gameObject.tag.Equals("Mirror"))
            {
                prompt.text = "";
                inSight = false;
                itemChecking = null;
            }
        }
        //private void HoldItem(Collider col)
        //{
        //    Vector3 offset = target.transform.forward;
        //    //offsetScale = (float)Mathf.Cos(Vector3.Angle(target.transform.forward, transform.forward) * Mathf.PI / 180);
        //    col.transform.position = target.transform.position + offset * 0.8f;
        //    //col.GetComponent<Rigidbody>().MovePosition(target.transform.position + offset);
        //    //col.transform.LookAt(target.transform.position+ target.transform.up*(col.transform.position.y- target.transform.position.y));
        //    col.transform.LookAt(target.transform.position);
        //}

        public void PlayerEnable(bool enable)
        {
            Cursor.visible = !enable;
            if (enable)
            {
                Cursor.lockState = CursorLockMode.Locked;
                GetComponent<RigidbodyFirstPersonController>().enableInput = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                GetComponent<RigidbodyFirstPersonController>().enableInput = false;
            }
            

        }

        public void GetItOut(int index)
        {

            if (!inventory[index])
                return;
            int i = 0;
            foreach (Image img in inventoryUI.GetComponentsInChildren<Image>())
            {
                if (img.tag != "ItemUI")
                    continue;
                if (i != index)
                {
                    i++;
                    continue;
                }
                Item item = img.GetComponent<Item>();
                if (item.isOn)
                {
                    img.gameObject.name = "empty";
                    img.sprite = UIMask;
                    item.isOn = false;

                    GameObject it = inventory[i];
                    inventory[i] = null;
                    it.transform.position = transform.position + transform.forward * 3;
                    it.SetActive(true);
                    break;

                }
            }
        }
        public void DestroyObj(int index)
        {
            if (!inventory[index])
                return;
            int i = 0;
            foreach (Image img in inventoryUI.GetComponentsInChildren<Image>())
            {
                if (img.tag != "ItemUI")
                    continue;
                if (i != index)
                {
                    i++;
                    continue;
                }
                Item item = img.GetComponent<Item>();
                if (item.isOn)
                {
                    img.gameObject.name = "empty";
                    img.sprite = UIMask;
                    item.isOn = false;

                    inventory[i] = null;
                    break;

                }
            }
        }

        public void ToggleInventory()
        {
            checkingInventory = !checkingInventory;
            if (checkingInventory)
            {
                PlayerEnable(false);
                inventoryAnim.SetBool("open", true);
                //inspectorAnim.SetBool("open", true);

            }
            else
            {
                if (book) book.SetActive(false);
                PlayerEnable(true);
                inventoryAnim.SetBool("open", false);
                //inspectorAnim.SetBool("open", false);

            }
            //yield return new WaitForSeconds(1);
        }

        private IEnumerator MirrorDone(Collider mirror, float sec)
        {
            mirror.tag = "Untagged";
            prompt.text = "";
            checkingMirror = false;
            target.transform.LookAt(mirror.gameObject.transform);
            StartCoroutine(ShowPrompt("Something happened...", sec));
            yield return new WaitForSeconds(sec);
            GetComponent<RigidbodyFirstPersonController>().enableInput = true;
            itemChecking = null;

        }

        private IEnumerator ShowPrompt(string text, float sec=2)
        {
            prompt.text = text;
            yield return new WaitForSeconds(sec);
            prompt.text = "";
        }


        private IEnumerator Delay(float sec)
        {
            yield return new WaitForSeconds(sec);
             
        }

    }
}
