using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]private GameObject m_HowToPlayPopUp;
    [SerializeField] private Text m_HighScoreText;
    void Start() //initialization the high score according to the last highScore
    {
        int highScore = PlayerPrefs.GetInt("HighScore",0);
        m_HighScoreText.text = string.Format("Highscore : {0}",highScore);
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

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
