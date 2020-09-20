using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]private GameObject m_HowToPlayPopUp;
    [SerializeField] private Text m_HighScoreText;
    AsyncOperation GameScene;
    void Start() //initialization the high score according to the last highScore
    {
        int highScore = PlayerPrefs.GetInt("HighScore",0);
        m_HighScoreText.text = string.Format("Highscore : {0}",highScore);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
