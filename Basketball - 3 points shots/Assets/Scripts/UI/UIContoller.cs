using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{
    private const int NumberOfPostions = 5;
    private const int NumberOfBallsToThrow = 20;
    [SerializeField] private Text m_RightBasketTimer;
    [SerializeField] private Text m_LeftBasketTimer;
    [SerializeField] private Text m_score;
    [SerializeField] private Canvas m_EndGameCanvas;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private gameController m_GameController;
    [SerializeField] private GameObject m_PowerBarCanvas;
    [SerializeField] private PowerBar m_PowerBar;
    public float TimeRemaining { get; private set; }
    public bool timerIsRunning = false;
    private Vector3 m_EndGameUIposition = new Vector3(-0.45f, -0.45f, -15.7f);
    private float[] m_EndGameUIRotationsOffsets = new float[5];
    private Canvas m_EndGameCanvasClone;
    private float m_SecondCounter = 0;
    
    void Start()
    {
        timerIsRunning = true;
        TimeRemaining = 16f;               //The player has 3 minutes.
        initializeEndGameUIRotationsOffsets();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            //Every one second reduce the time by one.
            m_SecondCounter += Time.deltaTime;
            if(m_SecondCounter > 1f)
            {
                TimeRemaining -= 1f;
                DisplayTime();
                m_SecondCounter = 0;
            }
            if(TimeRemaining == 11f)                //play the audio source 1 second before to  synchronize the audio and the time displayed.
            {
                //play the last ten second audio.
                FindObjectOfType<SoundController>().LastTenSeconds();
            }
            if (TimeRemaining <= 0f)    //The time is up - game finished.
            {
                timerIsRunning = false;
                m_GameController.EndGame();
            }
        }
    }
    //Display the time left on the timer according the TimeRemaining field.
    private void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(TimeRemaining / 60);                   //Calculate the minutes.
        float seconds = Mathf.FloorToInt(TimeRemaining % 60);                   //Calculate the seconds;
        m_RightBasketTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);        
        m_LeftBasketTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void UpdateScoreText()
    {
        m_score.text = string.Format("Score: {0}", m_GameController.m_pointsScored);
    }
    public void DisplayAndInitEndGameUI()
    {
        float rotateAngleOfUI = m_EndGameUIRotationsOffsets[4];                                            //Initialize the angle to make it appropriate to the last position of the game.

        m_EndGameCanvasClone = Instantiate(m_EndGameCanvas, m_EndGameUIposition, m_EndGameCanvas.transform.rotation);
        if(m_GameController.numberOfShootsThrown != NumberOfBallsToThrow)
        {
            rotateAngleOfUI = m_EndGameUIRotationsOffsets[(m_GameController.numberOfShootsThrown / 4)];    //Set the angle of the UI according to the current position.
        }
        if(m_EndGameCanvasClone != null)
        {
            m_EndGameCanvasClone.transform.RotateAround(m_EndGameCanvasClone.transform.position, Vector3.up, rotateAngleOfUI); //Rotate the EndGameUI according the appropriate angle.
            m_EndGameCanvasClone.transform.Find("EndGameImage").Find("ButtonRestart").GetComponent<Button>().onClick.AddListener(() => m_GameController.Restart());
            m_EndGameCanvasClone.transform.Find("EndGameImage").Find("ButtonQuit").GetComponent<Button>().onClick.AddListener(() => m_GameController.QuitGame());
            updateEndGameUIByResult();
        }
        
    }
    private void updateEndGameUIByResult()
    {
        int lastHighScore = PlayerPrefs.GetInt("HighScore");        //Get the last highscore.
        if (m_GameController.m_pointsScored > lastHighScore)         //Check if the player achieve new highscore.
        {
            m_EndGameCanvasClone.transform.Find("EndGameImage").Find("NewHighScore").gameObject.SetActive(true);        //Display the new highscore text.
            PlayerPrefs.SetInt("HighScore", m_GameController.m_pointsScored);                                           //Update the new highscore.       
        }
        string currentGameScoreText = string.Format("Your Score: {0}", m_GameController.m_pointsScored);
        m_EndGameCanvasClone.transform.Find("EndGameImage").Find("YourScore").GetComponent<Text>().text = currentGameScoreText;  //Update the current score.
    }
    private void initializeEndGameUIRotationsOffsets()
    { 
        float angle = 0f;
        for (int i = 0; i < NumberOfPostions; i++)                                   
        {
            m_EndGameUIRotationsOffsets[i] = angle; 
            angle += 45f;
        }
    }
    public void HidePowerbar()
    {
        m_PowerBar.enabled = false;                                          //Turn off the power bar.
        m_PowerBarCanvas.SetActive(false);                                   //Hide the powerBar Canvas.
    }
    public void DisplayPowerBar()
    {
        m_PowerBarCanvas.SetActive(true);               //Dispaye the power bar.
        m_PowerBar.TurnOnPowerBar();                    //When the player holding the ball we need to turn on the power bar.
    }
}
