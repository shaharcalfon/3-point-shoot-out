using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class PlayerMainMenu : MonoBehaviour
{
    [SerializeField]
    private Camera m_MainCamera;

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

            if (Physics.Raycast(myRay, out hit))
            {
                if(hit.collider != null)
                {
                    if(hit.transform.gameObject.name == "StartButton")
                    {
                        hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                    else if(hit.transform.gameObject.name == "QuitButton")
                    {
                        hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                    else if(hit.transform.gameObject.name == "HowToPlayButton")
                    {
                        hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                    }

                }
            }
        }
    }
}
