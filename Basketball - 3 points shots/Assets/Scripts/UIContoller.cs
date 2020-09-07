using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{
    [SerializeField] private Text m_Timer;
    [SerializeField] private Text m_score;
    private int m_pointsScored = 0;
    private float m_SecondCounter = 0;
    private float timeRemaining = 180;
    private bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            m_SecondCounter += Time.deltaTime;
            if(m_SecondCounter > 1)
            {
                timeRemaining -= 1;
                DisplayTime(timeRemaining);
                m_SecondCounter = 0;
            }
            if(timeRemaining==10)
            {
                //play the last ten second audio.
                FindObjectOfType<SoundController>().lastTenSeconds();
            }
            if (timeRemaining < 0) 
            {
                timerIsRunning = false;
            }
        }

    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        m_Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void addThreePoints()
    {
        m_pointsScored += 3;
        m_score.text = string.Format("Score: {0}", m_pointsScored);
    }
}
