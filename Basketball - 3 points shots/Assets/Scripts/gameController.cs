using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    [SerializeField] private UIContoller m_UIController; 
    public int numberOfShootsThrown = 0;

    private void Update()
    {

    }

    public void EndGame()
    {
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
