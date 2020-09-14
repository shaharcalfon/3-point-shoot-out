using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [SerializeField] private float m_BallOffset = 1.5f;
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private gameController m_GameController;
    [SerializeField] private GameObject m_PowerBarCanvas;
    [SerializeField] private PowerBar m_PowerBar;
    [SerializeField] private Animator m_RightHand;
    [SerializeField] private Animator m_LeftHand;

    private float m_BallThrowingForce = 515f;
    private float m_BallAngleForce = 180f;
    private Transform m_PlayerTransform;
    private Transform m_CurrentBall = null;
    private Vector3[] throwPositions = new Vector3[5];
    private bool holdingBall = false;



    // Start is called before the first frame update
    void Start()
    {
        initializeThrowPositions();
        m_PlayerTransform = transform;
    }
    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    void checkInput()
    {
        if(!m_GameController.isGameOn)                      //Press the UI Buttons.
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.name == "ButtonRestart")
                    {
                        hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                    if (hit.transform.gameObject.name == "ButtonQuit")
                    {
                        hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        }
        if (Input.anyKeyDown && holdingBall && m_GameController.isGameOn)
        {
            ThrowingBall();
        }
        else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftAlt) && !holdingBall&& m_GameController.isGameOn)
        {
            RaycastHit hit;
            Ray myRay = m_MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider != null)
                {
                    m_LeftHand.SetTrigger("Catch");
                    m_RightHand.SetTrigger("Catch");
                    Debug.Log("HIT!!!" + hit.transform.gameObject.name);
                    Debug.Log("HIT!!!" + hit.collider.transform.parent.name);
                    Debug.Log("Format" + string.Format("Balls rack-{0}", m_GameController.numberOfShootsThrown / 4 + 1));
                    //CheckCorrectBallRack(hit.collider.transform.parent.name);
                    if (hit.collider.tag == "Ball" && hit.collider.transform.parent.name == string.Format("Balls rack-{0}", m_GameController.numberOfShootsThrown / 4 + 1))
                    {
                        m_CurrentBall = hit.transform;                  //Update the ball reference to the current ball we hit.
                        setBallDetails();
                        holdingBall = true;
                        m_PowerBarCanvas.SetActive(true);               //Dispaye the power bar.
                        m_PowerBar.TurnOnPowerBar();                    //When the player holding the ball we need to turn on the power bar.
                    }
                    
                }
            }
        }
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

    //This method is fix the Ball details to be ready to throw.
    private void setBallDetails()
    {
        m_CurrentBall.SetParent(m_PlayerTransform);
        m_CurrentBall.transform.position = m_MainCamera.transform.position + m_MainCamera.transform.forward * m_BallOffset;
        m_CurrentBall.gameObject.AddComponent<Rigidbody>();
        m_CurrentBall.GetComponent<Rigidbody>().useGravity = false;

    }
    //This method Update all the relevant details when the player is throwing a ball.
    private void ThrowingBall()
    {
        m_PowerBar.enabled = false;                                          //Turn off the power bar.
        m_PowerBarCanvas.SetActive(false);                                   //Hide the powerBar Canvas.
        Debug.Log("" + m_PowerBar.m_FillAmount);
        m_CurrentBall.SetParent(null);                                       //The player not holding a ball.
        m_CurrentBall.GetComponent<Rigidbody>().useGravity = true;           //Update the useGravity field.
        float balancedPower = ThrowingPowerBalance(m_PowerBar.m_FillAmount);
        m_RightHand.SetTrigger("Throw");
        m_LeftHand.SetTrigger("Throw");
        m_CurrentBall.GetComponent<Rigidbody>().AddForce((m_MainCamera.transform.forward * m_BallThrowingForce * balancedPower) + m_MainCamera.transform.up * m_BallAngleForce);
        Destroy(m_CurrentBall.gameObject, 8);
        holdingBall = false;                                                //The player is not holding a ball.
        m_GameController.numberOfShootsThrown++;                             //Increase the number of shoots thrown.
        if (m_GameController.numberOfShootsThrown % 4 == 0 && m_GameController.numberOfShootsThrown <= 16) //Check if the player need to change position.
        {
            Invoke("changePosition", 2);    //Change the position after 2 second.
        }
        if (m_GameController.numberOfShootsThrown == 20)
        {
            m_GameController.EndGame();
        }
    }
    //This method balanced the throwing power to make it real throw.
    private float ThrowingPowerBalance(float i_FillAmount)
    {
        float balancedPower = 0;
        if (i_FillAmount >= 0.8f)
        {
            balancedPower = 1f;
        }
        else if (i_FillAmount < 0.8f && i_FillAmount >= 0.6f)
        {
            balancedPower = 0.8f;
        }
        else
        {
            balancedPower = 0.6f;
        }
        return balancedPower;

    }
    //This method return true only if the player trying to catch ball from the current balls rack else return false.
    private bool CheckCorrectBallRack(string i_BallRackName)
    {
        bool isCorrectRack = true;
        int currentBallRackNumber = m_GameController.numberOfShootsThrown / 4 + 1;
        string CurrentBallRackName = string.Format("Balls rack-{0}", currentBallRackNumber);
        Debug.Log("HIT!!!" + CurrentBallRackName);

        if (i_BallRackName != CurrentBallRackName)
        {
            isCorrectRack = false;
        }

        return isCorrectRack;
    }
    private void changePosition()
    {
        Vector3 nextPosition = throwPositions[(m_GameController.numberOfShootsThrown / 4)];
        m_MainCamera.transform.position = nextPosition;         //Update the main camera position with the next player position.
    }
}



