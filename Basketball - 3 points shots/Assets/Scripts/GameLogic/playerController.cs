using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    private const int NumberOfBallInRack = 4;
    private const int NumberOfBallsToThrow = 20;
    [SerializeField] private float m_BallOffset = 0.98f;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private Camera m_MainCamera;

    [SerializeField] private gameController m_GameController;
    [SerializeField] private PowerBar m_PowerBar;
    [SerializeField] private GameObject m_Hands;
    [SerializeField] private Animator m_HandsAnimator;
    [SerializeField] private Animator m_RightHandAnimator;
    [SerializeField] private Animator m_LeftHandAnimator;
    [SerializeField] private UIContoller m_UIController;

    public Transform m_CurrentBall;
    public bool holdingBall = false;
    private float m_BallThrowingForce = 515f;
    private float m_BallAngleForce = 180f;
    private Vector3[] throwPositions = new Vector3[5];
    private float m_XScreenOffset = 0.257f;
    private float m_YSceenOffset = 0.494f;

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
        if(Input.anyKeyDown && !m_GameController.isGameOn)                      //Press the UI Buttons.
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

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
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Ball" && CheckCorrectBallsRack(hit.collider.transform.parent.name))
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
        m_HandsAnimator.SetTrigger("Catch");                         //Trigger to play the catch animation.     
        m_CurrentBall.gameObject.AddComponent<Rigidbody>();          //Add rigidbody componnet to make it physical object.
        m_CurrentBall.GetComponent<Rigidbody>().useGravity = false;
        holdingBall = true;
        m_UIController.DisplayPowerBar();
    }

    //This method initialize the throwPosition array with the 5 thorwing position.
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
            Transform holdingBallToDestroy = m_CurrentBall;
            m_UIController.HidePowerbar();
            m_LeftHandAnimator.SetTrigger("Throw");                              //Turn on the throw animation.   
            m_RightHandAnimator.SetTrigger("Throw");
            Invoke("addForceToBall", 0.15f);
            Destroy(holdingBallToDestroy.gameObject, 7);                         //Destroy the ball 7 seconds after the ball is thrown.
            holdingBall = false;                                                 //The player is not holding a ball.
            m_GameController.numberOfShootsThrown++;                             //Increase the number of shoots thrown.
            if (m_GameController.numberOfShootsThrown % NumberOfBallInRack == 0 &&
                m_GameController.numberOfShootsThrown < NumberOfBallsToThrow)           //Check if the player need to change position.
            {
                Invoke("changePosition", 2);                                     //Change the position after 2 second.
            }
            if (m_GameController.numberOfShootsThrown == 20)                    //The player throw all the balls - the game is finished.
            {
                m_GameController.EndGame();                 
            }
        }
    }

    private void addForceToBall()
    {
        if (m_CurrentBall != null)
        {
            m_CurrentBall.GetComponent<Rigidbody>().useGravity = true;           //Update the useGravity field.
            float balancedPower = ThrowingPowerBalance(m_PowerBar.FillAmount); //Balance the throw power according the actual fill amount of the power bar.
            m_CurrentBall.GetComponent<Rigidbody>().AddForce((m_MainCamera.transform.forward * m_BallThrowingForce * balancedPower) + m_MainCamera.transform.up * m_BallAngleForce);
            m_CurrentBall.SetParent(null);                                      
            m_CurrentBall = null;                                               //The player not holding a ball.
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
        int currentBallsRackNumber = m_GameController.numberOfShootsThrown / NumberOfBallInRack + 1;             //Calculate the current number balls rack.
        string CurrentBallsRackName = string.Format("Balls rack-{0}", currentBallsRackNumber);

        if (i_BallRackName != CurrentBallsRackName)
        {
            isCorrectRack = false;
        }

        return isCorrectRack;
    }
    private void changePosition()
    {
        Vector3 nextPosition = throwPositions[(m_GameController.numberOfShootsThrown / NumberOfBallInRack)];
        m_Player.transform.position = nextPosition;                                                        //Update the main camera position with the next player position.
         FindObjectOfType<SoundController>().PlayNbaSound();                                                                      //Play the nba sound.
    }
    public void SetBallPosition()
    {
        if (m_CurrentBall != null)
        {
            m_CurrentBall.transform.position = m_MainCamera.transform.position + m_MainCamera.transform.forward * m_BallOffset;
        }

    }
    public void SetBallParent()
    {
        if (m_CurrentBall != null) 
        {
            m_CurrentBall.SetParent(m_Hands.transform);
        }
        
    }
}



