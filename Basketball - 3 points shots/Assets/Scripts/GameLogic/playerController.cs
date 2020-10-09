using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    private const int NumberOfBallInRack = 4;
    private const int NumberOfBallsToThrow = 20;
    private const float XScreenOffset = 0.275f;
    private const float YSceenOffset = 0.525f;
    private const float BallThrowingForce = 515f;
    private const float BallAngleForce = 180f;
    private const float BallPositionOffset = 0.8f;

    [SerializeField] private GameObject m_Player;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private SoundController m_SoundController;
    [SerializeField] private gameController m_GameController;
    [SerializeField] private UIContoller m_UIController;
    [SerializeField] private PowerBar m_PowerBar;
    [SerializeField] private GameObject m_Hands;
    [SerializeField] private Animator m_RightHandAnimator;
    [SerializeField] private Animator m_LeftHandAnimator;

    
    public Transform m_CurrentBall;
    public bool holdingBall = false;
    private Vector3[] throwPositions = new Vector3[5];
    
    void Start()
    {
        initializeThrowPositions();
    }
    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    void checkInput()
    {
        if(Input.anyKeyDown && !m_GameController.isGameOn)                      //Press the EndGameUI Buttons.
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(XScreenOffset * Screen.width, YSceenOffset * Screen.height, 0f));

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider != null)
                {
                    GameObject gameObjectHitted = hit.transform.gameObject;
                    if (gameObjectHitted.name == "ButtonRestart")
                    {
                        gameObjectHitted.GetComponent<Button>().onClick.Invoke();
                    }
                    if (gameObjectHitted.name == "ButtonQuit")
                    {
                        gameObjectHitted.GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        }
        if (Input.anyKeyDown && holdingBall && m_GameController.isGameOn)               //The player throw the ball.
        {
            throwingBall();
        }
        else if (Input.anyKeyDown && !holdingBall && m_GameController.isGameOn)   //The player try to catch ball
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(XScreenOffset * Screen.width, YSceenOffset * Screen.height, 0f));

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Ball" && hit.collider.transform.parent !=null && CheckCorrectBallsRack(hit.collider.transform.parent.name))
                    {
                        m_CurrentBall = hit.transform;                                         //Update the ball reference to the current ball we hit.
                        catchBall();
                    }
                }
            }
        }
    }

    private void catchBall()
    {
        m_CurrentBall.gameObject.AddComponent<Rigidbody>();          //Add rigidbody componnet to make it physical object.
        m_Hands.GetComponent<Animator>().SetTrigger("Catch");                         //Trigger to play the catch animation.     
        m_CurrentBall.GetComponent<Rigidbody>().useGravity = false;
        holdingBall = true;
        m_UIController.DisplayPowerBar();
    }

    //This method initialize the throwPosition array with the 5 thorwing position of the game.
    private void initializeThrowPositions()
    {
        throwPositions[0] = new Vector3(-9.94f, 0.71f, -16.071f);
        throwPositions[1] = new Vector3(-5.32f, 0.71f, -8.39f);
        throwPositions[2] = new Vector3(-0.4f, 0.71f, -6.6f);
        throwPositions[3] = new Vector3(6.15f, 0.71f, -8.74f);
        throwPositions[4] = new Vector3(9.8f, 0.71f, -16f);
    }
    //This method Update all the relevant details when the player is throwing a ball.
    private void throwingBall()
    {
        if(m_CurrentBall != null)
        {
            Transform holdingBallToDestroy = m_CurrentBall;                             //copy the reference of the current holding ball.
            m_UIController.HidePowerbar();
            m_LeftHandAnimator.SetTrigger("Throw");                                     //Turn on the throw animation.   
            m_RightHandAnimator.SetTrigger("Throw");
            Invoke("addForceToBall", 0.15f);                                            //Throw the ball with 0.15 mSeconds delay to synchronize the shoot with the animation 
            Destroy(holdingBallToDestroy.gameObject, 7);                                //Destroy the ball 7 seconds after the ball is thrown.
            holdingBall = false;                                                        //The player is not holding a ball.
            m_GameController.NumberOfShootsThrown++;                                    //Increase the number of shoots thrown.
            if (m_GameController.NumberOfShootsThrown % NumberOfBallInRack == 0 &&
                m_GameController.NumberOfShootsThrown < NumberOfBallsToThrow)           //Check if the player need to change position.
            {
                Invoke("changePosition", 2);                                            //Change the position after 2 second.
            }
            if (checkEndGame())                            
            {
                m_GameController.EndGame();                 
            }
        }
    }

    private bool checkEndGame()
    {
        return m_GameController.NumberOfShootsThrown == NumberOfBallsToThrow;             //The player throw all the balls - the game is finished.
    }
    private void addForceToBall()
    {
        if (m_CurrentBall != null)
        {
            m_CurrentBall.GetComponent<Rigidbody>().useGravity = true;                          //Update the useGravity field.
            m_CurrentBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;    //UnFreeze the ball positions and rotations.
            float balancedPower = ThrowingPowerBalance(m_PowerBar.FillAmount);                  //Balance the throw power according the actual fill amount of the power bar.
            m_CurrentBall.GetComponent<Rigidbody>().AddForce((m_MainCamera.transform.forward * BallThrowingForce * balancedPower) + m_MainCamera.transform.up * BallAngleForce);
            m_CurrentBall.SetParent(null);                                      
            m_CurrentBall = null;                                                               //The player not holding a ball.
        }
    }

    //This method balanced the throwing power to make it real throw.
    private float ThrowingPowerBalance(float i_FillAmount)
    {
        float balancedPower = 0;
        if (i_FillAmount >= 0.8f)                                       //High Power
        {
            balancedPower = 1f;
        }
        else if (i_FillAmount < 0.8f && i_FillAmount >= 0.6f)           //Medium power
        {
            balancedPower = 0.8f;
        }
        else                                                            //Low power
        {   
            balancedPower = 0.6f;
        }
        return balancedPower;

    }
    //This method return true only if the player trying to catch ball from the current balls rack else return false.
    private bool CheckCorrectBallsRack(string i_BallRackName)
    {
        bool isCorrectRack = true;
        int currentBallsRackNumber = m_GameController.NumberOfShootsThrown / NumberOfBallInRack + 1;             //Calculate the current number balls rack.
        string CurrentBallsRackName = string.Format("Balls rack-{0}", currentBallsRackNumber);

        if (i_BallRackName != CurrentBallsRackName)
        {
            isCorrectRack = false;
        }

        return isCorrectRack;
    }
    private void changePosition()
    {
        Vector3 nextPosition = throwPositions[(m_GameController.NumberOfShootsThrown / NumberOfBallInRack)];
        m_Player.transform.position = nextPosition;                                                        //Update the main camera position with the next player position.
        m_SoundController.PlayNbaSound();                                                                  //Play the nba sound.
    }

    //This method invoke on the hands catch animation.
    public void SetBallPosition()
    {
        if (m_CurrentBall != null)
        { 
            m_CurrentBall.transform.position = m_Hands.transform.position + m_Hands.transform.forward * BallPositionOffset;
            m_CurrentBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //freeze the ball position and rotation to prevent him to slip from the hands.       
        }

    }
    //This method invoke on the hands catch animation.
    public void SetBallParent()
    {
        if (m_CurrentBall != null) 
        {
            m_CurrentBall.SetParent(m_Hands.transform);
        }
        
    }
}



