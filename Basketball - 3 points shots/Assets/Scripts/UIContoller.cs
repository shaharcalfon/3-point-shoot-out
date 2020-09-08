using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{
    [SerializeField] private Text m_Timer;
    [SerializeField] private Text m_score;
    [SerializeField] private Image m_EndGameImage;
    [SerializeField] private Camera m_MainCamera;
    private int m_pointsScored = 0;
    private float m_SecondCounter = 0;
    private float m_timeRemaining = 180;
    public bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
    }


    void Update()
    {
        if (timerIsRunning)
        {
            //Every one second reduce the time by one.
            m_SecondCounter += Time.deltaTime;
            if(m_SecondCounter > 1)
            {
                m_timeRemaining -= 1;
                DisplayTime(m_timeRemaining);
                m_SecondCounter = 0;
            }
            if(m_timeRemaining == 10)
            {
                //play the last ten second audio.
                FindObjectOfType<SoundController>().LastTenSeconds();
            }
            if (m_timeRemaining < 0) 
            {
                timerIsRunning = false;
                FindObjectOfType<gameController>().EndGame();
            }
        }

    }
    //Display the time left.
    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        m_Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void AddThreePoints()
    {
        m_pointsScored += 3;
        m_score.text = string.Format("Score: {0}", m_pointsScored);
    }
    public void DisplayEndGameUI()
    {
        Instantiate(m_EndGameImage, m_MainCamera.transform.position + new Vector3(2f, 0f, 0f), Quaternion.identity, m_MainCamera.transform);
    }
}
