using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private float m_BallOffset= 1.5f;
    [SerializeField] private float m_BallThrowingForce = 450f;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private gameController m_GameController;


    private Transform m_PlayerTransform;
    private Transform m_CurrentBall = null;
    private Vector3[] throwPositions = new Vector3[5];
    private bool holdingBall = false;
    

    // Start is called before the first frame update
    void Start()
    {
        initializeThrowPositions();
        m_PlayerTransform = transform;
    }

  

    // Update is called once per frame
    void Update()
    {
        checkInput();

    }

    void checkInput()
    {
        if(Input.anyKeyDown && holdingBall)
        {
            m_CurrentBall.SetParent(null);
            m_CurrentBall.GetComponent<Rigidbody>().useGravity = true;
            m_CurrentBall.GetComponent<Rigidbody>().AddForce((m_MainCamera.transform.forward * m_BallThrowingForce)+m_MainCamera.transform.up*120);
            Destroy(m_CurrentBall.gameObject, 8);
            holdingBall = false;
            m_GameController.numberOfShotsThrown++;
            if (m_GameController.numberOfShotsThrown % 4 == 0 && m_GameController.numberOfShotsThrown<=16)
            {
                Vector3 nextPosition = throwPositions[(m_GameController.numberOfShotsThrown / 4)];
                m_MainCamera.transform.position = nextPosition;
            }
        }
        else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftAlt) && !holdingBall)
        {  
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(myRay, out hit))
            {
                if(hit.collider!= null)
                {
                    Debug.Log("HIT!!!"+ hit.transform.gameObject.name);
                    if (hit.collider.tag == "Ball")
                    {
                        m_CurrentBall = hit.transform;
                        setBallDetails();
                        holdingBall = true;                      
                    }
                    //if(hit.collider.tag=="Balls Rack")
                    //{
                    //   m_MainCamera.transform.position += m_MainCamera.transform.forward * 0.5f;
                    //}
                }               
            }
        }
    }
    private void initializeThrowPositions()
    {
        throwPositions[0] = new Vector3(-9.94f, 0.71f, -16.071f);
        throwPositions[1] = new Vector3(-5.66f, 0.71f, -7.97f);
        throwPositions[2] = new Vector3(-0.1f, 0.71f, -7.532f);
        throwPositions[3] = new Vector3(6.15f, 0.71f, -8.74f);
        throwPositions[4] = new Vector3(9.574f, 0.71f, -16.509f);
    }
    private void setBallDetails()
    {
        m_CurrentBall.SetParent(m_PlayerTransform);
        m_CurrentBall.transform.position = m_MainCamera.transform.position + m_MainCamera.transform.forward * m_BallOffset;
        m_CurrentBall.GetComponent<Rigidbody>().useGravity = false;

    }
}


