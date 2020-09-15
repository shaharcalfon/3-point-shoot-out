using System;
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
    [SerializeField] private gameController m_GameController;

    private const int NumberOfPostions = 5;
    private Vector3 m_EndGameUIposition = new Vector3(-0.45f, -0.45f, -15.7f);
    private float[] m_EndGameUIRotationsOffsets = new float[5];
    private Canvas m_EndGameCanvasClone;
    private float m_SecondCounter = 0;
    private float m_timeRemaining = 30;
    public bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        initializeEndGameUIRotationsOffsets();
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
                m_GameController.EndGame();
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
        m_score.text = string.Format("Score: {0}", m_GameController.m_pointsScored);
    }
    public void DisplayAndInitEndGameUI()
    {
        float rotateAngleOfUI = m_EndGameUIRotationsOffsets[4];                                                    //Initialize the angle  to make it appropriate to the last position.

        m_EndGameCanvasClone = Instantiate(m_EndGameCanvas, m_EndGameUIposition, m_EndGameCanvas.transform.rotation);
        if(m_GameController.numberOfShootsThrown!=20)
        {
            rotateAngleOfUI = m_EndGameUIRotationsOffsets[(m_GameController.numberOfShootsThrown / 4)];            //Set the angle of the UI according to the current position.
        }
        m_EndGameCanvasClone.transform.RotateAround(m_EndGameCanvasClone.transform.position,Vector3.up, rotateAngleOfUI); //Rotate the EndGameUI according the appropriate angle.
        m_EndGameCanvasClone.transform.Find("EndGameImage").Find("ButtonRestart").GetComponent<Button>().onClick.AddListener(() => m_GameController.Restart());
        m_EndGameCanvasClone.transform.Find("EndGameImage").Find("ButtonQuit").GetComponent<Button>().onClick.AddListener(() => m_GameController.QuitGame());
        updateEndGameUIByResult();
    }

    private void updateEndGameUIByResult()
    {
        int lastHighScore = PlayerPrefs.GetInt("HighScore");        //Get the last highscore.
        if(m_GameController.m_pointsScored>lastHighScore)           //Check if the player achieve new highscore.
        {
            m_EndGameCanvasClone.transform.Find("EndGameImage").Find("NewHighScore").gameObject.SetActive(true);        //Display the new highscore text.
            PlayerPrefs.SetInt("HighScore", m_GameController.m_pointsScored);
        }
        string currentGameScoreText = string.Format("Your Score: {0}", m_GameController.m_pointsScored);
        m_EndGameCanvasClone.transform.Find("EndGameImage").Find("YourScore").GetComponent<Text>().text = currentGameScoreText;  //Update the current score.
    }

    private void initializeEndGameUIRotationsOffsets()
    { 
        float angle = 0f;
        for(int i=0 ; i< NumberOfPostions; i++)
        {
            m_EndGameUIRotationsOffsets[i] = angle; 
           angle += 45f;
            Debug.Log("" + m_EndGameUIRotationsOffsets[i]);
        }
    }
}
