using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HowToPlayPopUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayHowToPlay(bool i_Active)
    {
        if(m_HowToPlayPopUp != null)
        {
            m_HowToPlayPopUp.SetActive(i_Active);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
