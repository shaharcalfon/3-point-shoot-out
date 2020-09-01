﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 m_StartPosition = new Vector3(-8.79f, 0.67f, -16.41f);
    private Transform m_PlayerTransform;
    private Transform m_Ball = null;
    [SerializeField] private Camera m_MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //Update the position of the camera to the start position.
        m_MainCamera.transform.position = m_StartPosition;
        m_PlayerTransform = transform;
        transform.position = m_StartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Ball!= null)
        {
            m_Ball.transform.position = m_MainCamera.transform.position + m_MainCamera.transform.forward;
        }
        checkInput();
    }

    void checkInput()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftAlt))
        {
             
            RaycastHit hit;
            Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            if (Physics.Raycast(myRay, out hit))
            {
                if(hit.collider!= null)
                {
                    Debug.Log("HIT!!!");
                    if (hit.collider.tag == "Ball")
                    {
                        hit.rigidbody.useGravity = false;
                        m_Ball = hit.transform;
                    }
                    if(hit.collider.tag =="Balls Rack")
                    {
                        m_PlayerTransform.position += m_PlayerTransform.forward * 0.5f;
                    }
                }
                
               

            }
        }
    }
}
