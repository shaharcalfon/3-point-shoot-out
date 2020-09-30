using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    [SerializeField] private UIContoller m_UIController;
    [SerializeField] private playerController m_PlayerController;
    public int m_pointsScored = 0;
    public int numberOfShootsThrown = 0;
    public bool isGameOn { get; set; }

    private void Start()
    {
        isGameOn = true;
    }

    public void EndGame()
    {
        isGameOn = false;
        m_UIController.timerIsRunning = false;          //Stop the timer.
        if (m_PlayerController.m_CurrentBall != null && m_UIController.TimeRemaining <= 0f)      //If the player hold a ball and the time is up, we need to destroy the ball.
        {
            m_UIController.HidePowerbar();
            Destroy(m_PlayerController.m_CurrentBall.gameObject);
        }
        m_UIController.updateHighScores();  //Update the HighScores Table according the current game score.
        Invoke("DisplayEndGameUI",5f);      //Display the end game UI when the game is finish with 3 second delay.
    }

    public void AddThreePoints()
    {
        m_pointsScored += 3;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void DisplayEndGameUI()
    {
        m_UIController.DisplayAndInitEndGameUI();
    }


 
}
