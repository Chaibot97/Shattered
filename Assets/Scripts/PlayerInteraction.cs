using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class PlayerInteraction : MonoBehaviour {


        private readonly int pickUpFOV = 80;
        //[SerializeField] AudioClip takeItem;
        //[SerializeField] AudioClip teleport;
        private Transform target;
        private bool holding;
        private Collider itemHolding;
        private Shader shader1;
        private Shader shader2;
        private Renderer rend;

        private List<GameObject> inventory;


        [SerializeField] public Canvas menuUI;
        LayoutGroup inventoryUI;
        //private Collider collider;
        private int cd;

        private void Start()
        {
            menuUI.enabled = false;
            holding = false;
            //collider = GetComponent<Collider>();
            cd = 60;
            target = Camera.main.transform;
            shader1 = Shader.Find("Diffuse");
            shader2 = Shader.Find("Shader Learn/OutLighting");
            inventory = new List<GameObject>(3);
            //inventory[0] = new GameObject();
            foreach (LayoutGroup lp in menuUI.GetComponentsInChildren<LayoutGroup>())
            {
                if (lp.name.Equals("Inventory"))
                {
                    inventoryUI = lp;
                }
            }

        }

        private void Update()
        {


            if (itemHolding && holding)
            {
                HoldItem(itemHolding);
            }
            cd++;

            if (Input.GetKey(KeyCode.Tab))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                menuUI.enabled = true;
                GetComponent<RigidbodyFirstPersonController>().enabled=false;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                menuUI.enabled = false;
                GetComponent<RigidbodyFirstPersonController>().enabled = true;

            }

            if (Input.GetKeyUp(KeyCode.O))
            {
                int i = 0;
                foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
                {
                    if (lp == inventoryUI)
                        continue;
                    Toggle checker = lp.GetComponentInChildren<Toggle>();
                    if (checker.isOn)
                    {
                        lp.GetComponentInChildren<Text>().text = "empty";
                        lp.GetComponentsInChildren<Image>()[1].sprite = null;
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
            Vector3 direction = col.transform.position - target.transform.position;
            float angle = Vector3.Angle(direction, target.transform.forward);
            if (cd >= 60 && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E)))
            {
                if (angle <= pickUpFOV * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + transform.up * 0.2f, direction.normalized, out hit, 5))
                    {
                        if (col.gameObject.tag.Contains("Pickupable"))
                        {
                            //rend = col.GetComponent<Renderer>();
                            //rend.material.shader = shader2;

                            cd = 0;
                            int i = 0;

                            foreach (LayoutGroup lp in inventoryUI.GetComponentsInChildren<LayoutGroup>())
                            {
                                if (lp == inventoryUI)
                                    continue;
                                Toggle checker = lp.GetComponentInChildren<Toggle>();
                                if (!checker.isOn)
                                {
                                    lp.GetComponentInChildren<Text>().text = col.gameObject.name;
                                    lp.GetComponentsInChildren<Image>()[1].sprite = col.gameObject.GetComponent<Image>().sprite;
                                    checker.isOn = true;
                                    Debug.Log(i);
                                    inventory.Insert(i, col.gameObject);
                                    col.gameObject.SetActive(false);
                                    break;

                                }
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    //rend.material.shader = shader1;
                }
            }


        }

        void OnTriggerExit(Collider col)
        {
            //rend.material.shader = shader1;
        }
        private void HoldItem(Collider col)
        {
            Vector3 offset = target.transform.forward;
            //offsetScale = (float)Mathf.Cos(Vector3.Angle(target.transform.forward, transform.forward) * Mathf.PI / 180);
            col.transform.position = target.transform.position + offset * 0.8f;
            //col.GetComponent<Rigidbody>().MovePosition(target.transform.position + offset);
            //col.transform.LookAt(target.transform.position+ target.transform.up*(col.transform.position.y- target.transform.position.y));
            col.transform.LookAt(target.transform.position);
        }

    }
}
