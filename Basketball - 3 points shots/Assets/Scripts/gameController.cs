using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public int numberOfShotsThrown = 0;


    public void EndGame()
    {

    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }


 
}
