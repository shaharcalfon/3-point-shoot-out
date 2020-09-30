using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_HowToPlayPopUp;
    [SerializeField] private GameObject m_HighScoresPopUp;
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_LoadingScreen;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Text m_ProgressPercentageText;
    [SerializeField] private Text[] PlayersScoreArray;

    private const int numberOfScoresInTheTable = 10;

    AsyncOperation GameScene;
    void Start() //initialization the highscores.
    {
        InitializeHighScoresTabel();
    }
    public void DisplayHowToPlay(bool i_Active)
    {
        if(m_HowToPlayPopUp != null)
        {
            m_HowToPlayPopUp.SetActive(i_Active);
        }
    }
    public void DisplayHighScores(bool i_Active)
    {
        if (m_HighScoresPopUp != null)
        {
            m_HighScoresPopUp.SetActive(i_Active);
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
        Invoke("newGameCoroutine", 0.5f);                  //Load asynchronously game scene after 1 second.
    }
    

    private void newGameCoroutine()
    {
        StartCoroutine(loadGameAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }
     IEnumerator loadGameAsynchronously(int i_BuildIndex)
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
    private void InitializeHighScoresTabel()
    {
        string Place;
        for (int i = 0; i < numberOfScoresInTheTable; i++) 
        {
            Place = string.Format("{0}", i + 1);
            PlayersScoreArray[i].text = PlayerPrefs.GetInt(Place, 0).ToString();
        }
    }
}
