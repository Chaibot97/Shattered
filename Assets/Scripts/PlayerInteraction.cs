using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class PlayerInteraction : MonoBehaviour {


        private readonly int pickUpFOV = 60;
        //[SerializeField] AudioClip takeItem;
        //[SerializeField] AudioClip teleport;
        private Transform target;
        private bool holding;

        private Collider itemChecking;
        private Shader shader1;
        private Shader shader2;
        private Renderer rend;

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

        private int playerLayerMask=1<<9;
        private void Start()
        {
            holding = false;
            cd = 30;
            target = Camera.main.transform;
            shader1 = Shader.Find("Standard (Roughness setup)");
            shader2 = Shader.Find("Shader_highlight/0.TheFirstShader");
            inventory = new List<GameObject>(3);
            for (int i = 0; i < 3; i++) 
                inventory.Add(new GameObject());

            inventoryAnim = inventoryUI.GetComponent<Animator>();
            inspectorAnim = inspectorUI.GetComponent<Animator>();


        }

      
        private void FixedUpdate()
        {


            if(cd<30)
                cd++;
            if (!checkingMirror)
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
            else
            {

                target.transform.LookAt(itemChecking.gameObject.transform);
                Vector3 direction = transform.position- itemChecking.transform.position;
                float angle = Vector3.SignedAngle(direction, itemChecking.transform.up,Vector3.up);
                itemChecking.GetComponent<MirrorTrigger>().reveal(angle);
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
                        prompt.enabled = true;
                        prompt.text = "Press E to inspect.";
                        if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
                        {
                            cd = 0;
                            itemChecking = col;
                            //prompt.enabled = false;
                            checkingMirror = true;

                            //GetComponent<RigidbodyFirstPersonController>().enabled = false;
                            transform.position=(col.gameObject.transform.position + col.gameObject.transform.up * 1);
                            //transform.LookAt(col.gameObject.transform);
                            target.transform.LookAt(col.gameObject.transform);
                            GetComponent<RigidbodyFirstPersonController>().enableInput = false;
                       
                            prompt.text = "Press A/D to change angle. Press E to quit.";
                            return;

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
                
                Vector3 direction = col.transform.position - target.transform.position;
                float angle = Vector3.Angle(direction, target.transform.forward);
                // Debug.Log(angle);

                itemChecking = col;

                RaycastHit hit;
                if (Physics.Raycast(target.transform.position, target.transform.forward, out hit,playerLayerMask, 5))
                {
                    if(hit.collider.Equals(col)){
                        inSight = true;
                        rend = col.GetComponent<Renderer>();
                        rend.material.shader = shader2;
                        if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.E)))
                        {
                            cd = 0;
                            col.gameObject.GetComponent<Interactable>().interact();
                        }
                    }
                }
            
            }
            else if (col.gameObject.tag.Equals("Pickupable"))
            {
                Vector3 direction = col.transform.position - target.transform.position;
                float angle = Vector3.Angle(direction, target.transform.forward);
                itemChecking = col;
                if (angle <= pickUpFOV * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(target.transform.position, direction.normalized, out hit,playerLayerMask, 5))
                    {
                        if(hit.collider.Equals(col)){
                            inSight = true;
                            rend = col.GetComponent<Renderer>();
                            rend.material.shader = shader2;
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
                                        break;

                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
              


            }

            if (rend && !inSight)
            {
                if (col.gameObject.tag.Equals("Pickupable") || col.gameObject.tag.Equals("Interactable"))
                {
                    rend.material.shader = shader1;
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
            if (col.gameObject.tag.Equals("Pickupable") || col.gameObject.tag.Equals("Interactable"))
            {
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

    }
}
