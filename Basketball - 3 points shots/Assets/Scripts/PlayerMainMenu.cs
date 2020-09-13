﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class PlayerMainMenu : MonoBehaviour
{
    [SerializeField]
    private Camera m_MainCamera;
    [SerializeField]
    private GameObject m_HowToPlayPopUp;


    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        if(Input.anyKeyDown)
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if(Physics.Raycast(myRay, out hit))
            {
                string colliderName = hit.transform.gameObject.name;
                Button button = hit.transform.gameObject.GetComponent<Button>();
                if(hit.collider != null && isValidClicked(colliderName))
                {
                    button.onClick.Invoke();
                }
            }
        }
    }

    private bool isValidClicked(string i_ColliderName)
    {
        bool validClicked = false;
        if(i_ColliderName == "ButtonStart" || i_ColliderName == "ButtonQuit" || i_ColliderName == "ButtonHowToPlay")
        {
             if ((m_HowToPlayPopUp!=null) && (m_HowToPlayPopUp.activeSelf == false))
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

