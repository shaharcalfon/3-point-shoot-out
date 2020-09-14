using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    [SerializeField] private UIContoller m_UIController;
    [SerializeField] private playerController m_PlayerController;
    public int numberOfShootsThrown = 0;
    public bool isGameOn=true;

    private void Update()
    {

    }

    public void EndGame()
    {
        isGameOn = false;
        FindObjectOfType<UIContoller>().DisplayEndGameUI();
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
