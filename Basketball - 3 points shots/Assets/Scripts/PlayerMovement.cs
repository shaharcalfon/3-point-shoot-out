using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 m_StartPosition = new Vector3(-8.79f, 0.67f, -16.41f);
    [SerializeField] private float m_BallOffset= 1.5f;
    
    private Transform m_PlayerTransform;
    private Transform m_Ball = null;
    [SerializeField] private Camera m_MainCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        //Update the position of the camera to the start position.
       // m_MainCamera.transform.position = m_StartPosition;
        m_PlayerTransform = m_MainCamera.transform;
      // m_PlayerTransform.position = m_StartPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Ball!= null)
        {
            m_Ball.transform.position = m_MainCamera.transform.position + m_MainCamera.transform.forward * m_BallOffset;
        }
        checkInput();
        
    }

    void checkInput()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftAlt))
        {
             
            RaycastHit hit;
            Debug.Log(""+ Screen.width / 2f + "," + Screen.height / 2f);
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            
            if (Physics.Raycast(myRay, out hit))
            {
                if(hit.collider!= null)
                {
                    Debug.Log("HIT!!!"+ hit.transform.gameObject.tag);
                    if (hit.collider.tag == "Ball")
                    {
                        hit.rigidbody.useGravity = false;
                        m_Ball = hit.transform;
                    }
                    if(hit.collider.tag =="Balls Rack")
                    {
                        m_PlayerTransform.position += m_PlayerTransform.forward * 0.5f;
                    }
                    if (hit.collider.tag == "Floor")
                    {
                        m_PlayerTransform.position += m_PlayerTransform.forward * 0.5f;
                    }

                }               
            }
        }
    }
  

}


