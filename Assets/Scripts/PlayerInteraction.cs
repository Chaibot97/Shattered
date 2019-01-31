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

        [SerializeField] public Canvas inventoryUI;
        private Animator inventoryAnim;
        [SerializeField] public Canvas inspectorUI;
        private Animator inspectorAnim;
        [SerializeField] public Sprite UIMask;

        //LayoutGroup inventoryUI;
        private int cd;

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

            if (Input.GetKey(KeyCode.Tab))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                inventoryAnim.SetBool("open",true);
                inspectorAnim.SetBool("open", true);
                GetComponent<RigidbodyFirstPersonController>().enabled=false;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                inventoryAnim.SetBool("open", false);
                inspectorAnim.SetBool("open", false);
                GetComponent<RigidbodyFirstPersonController>().enabled = true;

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
            if (col.gameObject.tag.Equals("Pickupable")|| col.gameObject.tag.Equals("Interaction"))
            {
                Vector3 direction = col.transform.position - target.transform.position;
                float angle = Vector3.Angle(direction, target.transform.forward);
                itemChecking = col;
                if (angle <= pickUpFOV * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + transform.up * 0.2f, direction.normalized, out hit, 5))
                    {

                        inSight = true;
                        rend = col.GetComponent<Renderer>();
                        rend.material.shader = shader2;
                        if (cd >= 30 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
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

            if (rend && !inSight)
            {
                rend.material.shader = shader1;
                itemChecking = null;
            }

        }

        void OnTriggerExit(Collider col)
        {
            inSight = false;
            itemChecking = null;
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
