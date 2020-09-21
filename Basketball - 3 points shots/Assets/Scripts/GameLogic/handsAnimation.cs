using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handsAnimation : MonoBehaviour
{
    public void SetBallParent()
    {
        FindObjectOfType<playerController>().SetBallParent();
    }
    public void SetBallPosition()
    {
        FindObjectOfType<playerController>().SetBallPosition();
    }
}
