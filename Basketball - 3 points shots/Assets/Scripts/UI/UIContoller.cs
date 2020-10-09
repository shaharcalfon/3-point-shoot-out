using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{
    private const int NumberOfPostions = 5;
    private const int NumberOfBallsToThrow = 20;
    private const int NumberOfScoresInTheTable = 10;

    [SerializeField] private Text m_RightBasketTimer;
    [SerializeField] private Text m_score;
    [SerializeField] private Canvas m_EndGameCanvas;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private gameController m_GameController;
    [SerializeField] private SoundController m_SoundController;
    [SerializeField] private GameObject m_PowerBarCanvas;
    [SerializeField] private PowerBar m_PowerBar;
    public float TimeRemaining { get; private set; }
    public bool timerIsRunning = false;
    private Vector3 m_EndGameUIposition = new Vector3(0f, 0f, -15.75f);
    private float[] m_EndGameUIRotationsOffsets = new float[5];
    private Canvas m_EndGameCanvasClone;
    private float m_SecondCounter = 0;
    
    void Start()
    {
        timerIsRunning = true;
        TimeRemaining = 180f;                    //The player has 3 minutes.
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
                TimeRemaining -= 1f;                //Reduce the time remaining.
                DisplayTime();
                m_SecondCounter = 0;
            }
            if(TimeRemaining == 11f)                //play the audio source 1 second before to synchronize the audio and the time displayed.
            {
                m_SoundController.LastTenSeconds();
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
    }
    public void UpdateScoreText()
    {
        m_score.text = string.Format("Score: {0}", m_GameController.m_pointsScored);
    }
    public void DisplayAndInitEndGameUI()
    {
        float rotateAngleOfUI = m_EndGameUIRotationsOffsets[4];                      //Initialize the angle to make it appropriate to the last position in the game.

        m_EndGameCanvasClone = Instantiate(m_EndGameCanvas, m_EndGameUIposition, m_EndGameCanvas.transform.rotation);
        if(m_GameController.NumberOfShootsThrown != NumberOfBallsToThrow)           //The player is not at the last throw position.
        {
            rotateAngleOfUI = m_EndGameUIRotationsOffsets[(m_GameController.NumberOfShootsThrown / 4)];    //Set the angle of the UI according to the current position.
        }
        if(m_EndGameCanvasClone != null)
        {
            m_EndGameCanvasClone.transform.RotateAround(m_EndGameCanvasClone.transform.position, Vector3.up, rotateAngleOfUI); //Rotate the EndGameUI according the appropriate angle.
            GameObject.FindWithTag("ButtonRestart").GetComponent<Button>().onClick.AddListener(() => m_GameController.Restart());
            GameObject.FindWithTag("ButtonQuit").GetComponent<Button>().onClick.AddListener(() => m_GameController.QuitGame());
            updateEndGameUIByResult();
        }
        
    }
    private void updateEndGameUIByResult()
    {
        int lastHighScore = PlayerPrefs.GetInt("1", 0);                                                  //Get the highscore.
        if (m_GameController.m_pointsScored > lastHighScore)                                             //Check if the player achieve new highscore.
        {
            GameObject.FindWithTag("HighScoreText").SetActive(true);                                     //Display the new highscore text.                          
        }
        string currentGameScoreText = string.Format("Your Score: {0}", m_GameController.m_pointsScored);
        GameObject.FindWithTag("ScoreText").GetComponent<Text>().text = currentGameScoreText;            //Update the current score.
    }
    public void updateHighScores()
    {
        if (checkCurrentGameScoreGetInHighScores())
        {
            int[] HighScore = new int[10];              
            initializeHighScores( HighScore);       
            FixHighScores(HighScore);
            updatePlayerPrefs(HighScore);
        }

    }
    //This method checks if the player achieved a score that get inside the High Score table.
    private bool checkCurrentGameScoreGetInHighScores()
    {
        int lastScore = PlayerPrefs.GetInt("10");           //Get the 10th score in the table.

        return m_GameController.m_pointsScored > lastScore;
    }
    //Initialize ints array with the highscore table values we saved, using player prefs.
    private void initializeHighScores(int[] i_HighScores)
    {
        string Place;
        for (int i = 0; i < NumberOfScoresInTheTable; i++)
        {
            Place = string.Format("{0}", i + 1);
            i_HighScores[i] = PlayerPrefs.GetInt(Place,0);
        }
        i_HighScores[NumberOfScoresInTheTable - 1] = m_GameController.m_pointsScored;       //The player score more than the 10th value in the table.

    }
    //This method fix the highscore table,the 10th score is a new value in the table
    private void FixHighScores(int[] i_HighScores)
    {
        int temp;
        for (int i = NumberOfScoresInTheTable - 1; i > 0; i--)       
        {
            if (i_HighScores[i] > i_HighScores[i - 1])
            {
                temp = i_HighScores[i];
                i_HighScores[i] = i_HighScores[i - 1];
                i_HighScores[i - 1] = temp;
            }
        }
    }
    private void updatePlayerPrefs(int[] i_HighScores)
    {
        string Place;
        for (int i = 0; i < NumberOfScoresInTheTable; i++)
        {
            Place = string.Format("{0}", i + 1);
            PlayerPrefs.SetInt(Place, i_HighScores[i]);
        }
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
