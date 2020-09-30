﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerMainMenu : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private GameObject m_HowToPlayPopUp;
    [SerializeField] private GameObject m_HighScoresPopUp;
    private float m_XScreenOffset = 0.1f;
    private float m_YSceenOffset = 0.15f;

    void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        if(Input.anyKeyDown)
        {
            Debug.Log("Any Key Down");
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3((Screen.width / 4f) - (m_XScreenOffset * Screen.width),
                (Screen.height / 2f) - (m_YSceenOffset * Screen.height), 0f));
            Debug.Log("Width: " + Screen.width / 4);

            if(Physics.Raycast(myRay, out hit))
            {
                Debug.Log("" + hit.transform.gameObject.name);
                string colliderName = hit.transform.gameObject.name;                
                if(hit.collider != null && isValidClicked(colliderName))
                {
                    hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
    private bool isValidClicked(string i_ColliderName)
    {
        bool validClicked = false;
        if(i_ColliderName == "ButtonStart" || i_ColliderName == "ButtonQuit" || i_ColliderName == "ButtonHowToPlay" || i_ColliderName == "ButtonHighScores")
        {
            if ((m_HowToPlayPopUp.activeSelf == false) && (m_HighScoresPopUp.activeSelf == false))                
            {
                validClicked = true;
            }
        }
        else if(i_ColliderName == "ButtonExit")                                                           
        {
            validClicked = true;
        }

        return validClicked;
    }
}

