using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] public Canvas inventoryUI;
        private Animator inventoryAnim;
        [SerializeField] public Canvas inspectorUI;
        private Animator inspectorAnim;
        [SerializeField] public Sprite UIMask;

        [SerializeField] public Text prompt;

        //LayoutGroup inventoryUI;
        private int cd;
        private int cd_sound;
        private int wait;

        private int playerLayerMask=1<<9;
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


        }

      
        private void FixedUpdate()
        {

            if(cd<30)
                cd++;
            if (cd_sound < 180)
                cd_sound++;
            if (wait < 180)
                wait++;
            if (!checkingMirror)
            {
                if (!islooking)
                {
                    if (Input.GetKey(KeyCode.Tab))
                    {
                        PlayerEnable(false);
                        inventoryAnim.SetBool("open", true);
                        inspectorAnim.SetBool("open", true);
                    }
                    else
                    {
                        PlayerEnable(true);
                        inventoryAnim.SetBool("open", false);
                        inspectorAnim.SetBool("open", false);
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
                    itemChecking.tag = "Untagged";
                    prompt.enabled = false;
                    checkingMirror = false;
                    GetComponent<RigidbodyFirstPersonController>().enableInput = true;
                    target.transform.LookAt(itemChecking.gameObject.transform);
                    itemChecking=null;
                }
                if (Input.GetKey(KeyCode.D)&& angle<35)
                {
                    GetComponent<Rigidbody>().AddForce(transform.right * 2f, ForceMode.Impulse);
                }
                if (Input.GetKey(KeyCode.A) && angle >- 35)
                {
                    GetComponent<Rigidbody>().AddForce(transform.right * -2f, ForceMode.Impulse);
                }
                //Debug.Log(angle);
                
            }

            if (Input.GetKeyUp(KeyCode.O))
            {
                int i = 0;
                foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
                {
                    if (lp.tag != "ItemUI")
                        continue;
                    Toggle checker = lp.GetComponentInChildren<Toggle>();
                    if (checker.isOn)
                    {
                        lp.gameObject.name = "Empty";
                        lp.GetComponentsInChildren<Image>()[1].sprite = UIMask;
                        checker.isOn = false;

                        GameObject item = inventory[i];
                        inventory[i] = null;
                        item.transform.position = transform.position + transform.forward * 3;
                        item.SetActive(true);
                        break;

                    }
                    i++;
                }
            }
        }

        void OnTriggerEnter(Collider col)
        {

        }

        void OnTriggerStay(Collider col)
        {
            inSight = false;
            if (itemChecking && col != itemChecking) return;
            if (col.gameObject.tag.Equals("Mirror"))
            {
                itemChecking = col;
                if (!checkingMirror)
                {
                    Vector3 direction = col.transform.position - target.transform.position;
                    float angle = Vector3.Angle(direction, target.transform.forward);
                    
                    if (angle <= pickUpFOV * 0.8f)
                    {
                        RaycastHit hit;
                        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
                        if (Physics.Raycast(col.transform.position, direction.normalized*-1, out hit, 5))
                        {
                            Debug.Log(hit.collider.name);
                            if (hit.collider.tag.Contains("Player"))
                            {
                                inSight = true;
                                prompt.enabled = true;
                                prompt.text = "Press E to inspect.";
                                if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
                                {
                                    cd = 0;
                                    itemChecking = col;
                                    //prompt.enabled = false;
                                    checkingMirror = true;

                                    //GetComponent<RigidbodyFirstPersonController>().enabled = false;
                                    transform.position = (col.gameObject.transform.position + col.gameObject.transform.up * 1.3f);
                                    //transform.LookAt(col.gameObject.transform);
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
                        prompt.enabled = false;
                    }
                }
                else
                {
                    if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
                    {
                        cd = 0;
                        prompt.enabled = false;
                        checkingMirror = false;
                        //GetComponent<RigidbodyFirstPersonController>().enabled = true;

                        GetComponent<RigidbodyFirstPersonController>().enableInput = true;

                    }
                }
            }else if (col.gameObject.tag.Equals("Interactable"))
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
                                    if (!photo_pickedup)
                                    {
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
                                Interactable i = col.gameObject.GetComponent<Interactable>();
                                if (!i.requirement)
                                {
                                    i.Interact();
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
                                }
                            }
                        }
                        
                    }
                }
            
            }
            else if (col.gameObject.tag.Equals("Pickupable"))
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
                        Debug.Log(hit.collider.tag);
                        if (hit.collider.tag=="Pickupable"){
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
                                int i = 0;
                                foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
                                {
                                    if (lp.tag != "ItemUI")
                                        continue;
                                    Toggle checker = lp.GetComponentInChildren<Toggle>();
                                    if (!checker.isOn)
                                    {
                                        lp.gameObject.name = col.gameObject.name;
                                        Image img = lp.GetComponentsInChildren<Image>()[1];
                                        if(col.gameObject.GetComponent<Image>())
                                            img.sprite = col.gameObject.GetComponent<Image>().sprite;
                                        checker.isOn = true;
                                        Debug.Log(i);
                                        inventory[i] = col.gameObject;
                                        col.gameObject.SetActive(false);
                                        inSight = false;
                                        itemChecking = null;
                                        putin.Play();
                                        if(col.gameObject.transform.parent.gameObject.name.Equals("photo 1")){
                                            photo_pickedup = true;
                                            col.gameObject.transform.parent.gameObject.SetActive(false);
                                        }
                                        break;

                                    }
                                    i++;
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
                if (col.gameObject.tag.Equals("Pickupable") || col.gameObject.tag.Equals("Interactable"))
                {
                    
                    itemChecking = null;
                }
                if (col.gameObject.tag.Equals("Mirror"))
                {
                    prompt.enabled = false;
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
            if (col.gameObject.tag.Equals("Pickupable") || col.gameObject.tag.Equals("Interactable"))
            {    

                inSight = false;
                itemChecking = null;
            }
            if (col.gameObject.tag.Equals("Mirror"))
            {
                prompt.enabled = false;
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
            foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
            {
                if (lp.tag != "ItemUI")
                    continue;
                if (i != index)
                {
                    i++;
                    continue;
                }
                Toggle checker = lp.GetComponentInChildren<Toggle>();
                if (checker.isOn)
                {
                    lp.gameObject.name = "empty";
                    lp.GetComponentsInChildren<Image>()[1].sprite = UIMask;
                    checker.isOn = false;

                    GameObject item = inventory[i];
                    inventory[i] = null;
                    item.transform.position = transform.position + transform.forward * 3;
                    item.SetActive(true);
                    break;

                }
            }
        }
        public void DestroyObj(int index)
        {
            if (!inventory[index])
                return;
            int i = 0;
            foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
            {
                if (lp.tag != "ItemUI")
                    continue;
                if (i != index)
                {
                    i++;
                    continue;
                }
                Toggle checker = lp.GetComponentInChildren<Toggle>();
                if (checker.isOn)
                {
                    lp.gameObject.name = "empty";
                    lp.GetComponentsInChildren<Image>()[1].sprite = UIMask;
                    checker.isOn = false;

                    GameObject item = inventory[i];
                    inventory[i] = null;
                    break;

                }
            }
        }

        private IEnumerator Delay(float sec)
        {
            yield return new WaitForSeconds(sec);
             
        }

    }
}
