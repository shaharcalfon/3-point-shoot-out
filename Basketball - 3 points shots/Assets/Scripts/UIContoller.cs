using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{
    [SerializeField] private Text m_Timer;
    [SerializeField] private Text m_score;
    [SerializeField] private Canvas m_EndGameCanvas;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private gameController m_gameController;

    //private Vector3[] EndGamePopUpOffsets = new Vector3[5];
    private Canvas m_EndGameImageClone;
    private float m_SecondCounter = 0;
    private float m_timeRemaining = 10;
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
    public void UpdateScoreText()
    {
        m_score.text = string.Format("Score: {0}", m_gameController.m_pointsScored);
    }
    public void DisplayAndInitEndGameUI()
    {
        m_EndGameImageClone = Instantiate(m_EndGameCanvas, m_MainCamera.transform.position + new Vector3(m_MainCamera.transform.forward.x*5,0, m_MainCamera.transform.forward.z),m_EndGameCanvas.transform.rotation);
        m_EndGameImageClone.transform.Find("EndGameImage").Find("ButtonRestart").GetComponent<Button>().onClick.AddListener(() => m_gameController.Restart());
        m_EndGameImageClone.transform.Find("EndGameImage").Find("ButtonQuit").GetComponent<Button>().onClick.AddListener(() => m_gameController.QuitGame());
    }
}
