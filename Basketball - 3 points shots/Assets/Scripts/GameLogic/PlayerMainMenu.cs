using UnityEngine;
using UnityEngine.UI;

public class PlayerMainMenu : MonoBehaviour
{
    private const float XScreenOffset = 0.275f;
    private const float YSceenOffset = 0.525f;

    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private GameObject m_HowToPlayPopUp;
    [SerializeField] private GameObject m_HighScoresPopUp;
    
    void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        if(Input.anyKeyDown)
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(XScreenOffset * Screen.width, YSceenOffset * Screen.height, 0f));
            if(Physics.Raycast(myRay, out hit))
            {         
                if(hit.collider != null && isValidClicked(hit.transform.gameObject.name))       
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
        else if(i_ColliderName == "ButtonExit" && ((m_HowToPlayPopUp.activeSelf == true) || (m_HighScoresPopUp.activeSelf == true)))      // The player try to close pop up.                                                     
        {
            validClicked = true;
        }

        return validClicked;
    }
}

