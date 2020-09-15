using System.Collections;
using System.Collections.Generic;
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
        m_UIController.timerIsRunning = false;
        m_UIController.DisplayAndInitEndGameUI();
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


 
}
