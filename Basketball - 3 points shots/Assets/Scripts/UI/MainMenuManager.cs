using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_HowToPlayPopUp;
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private Text m_HighScoreText;
    [SerializeField] private GameObject m_LoadingScreen;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Text m_ProgressPercentageText;

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
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        m_MainMenu.SetActive(false);
        m_LoadingScreen.SetActive(true);
        Invoke("newCoroutine", 1);                  //Load asynchronously game scene after 1 second.
    }
    private void newCoroutine()
    {
        StartCoroutine(loadLevelAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }
     IEnumerator loadLevelAsynchronously(int i_BuildIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(i_BuildIndex);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            m_Slider.value = progress;
            m_ProgressPercentageText.text = (progress * 100f).ToString("0") + "%";

            yield return null;
        }
    }
}
