using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCheck : MonoBehaviour
{
    [SerializeField] private Text FPS_Text;
    private int FrameCounter = 0;
    private float TimeSum = 0;

    // Update is called once per frame
    void Update()
    {
        TimeSum += Time.deltaTime;
        FrameCounter++;
        if(FrameCounter>7)
        {
            FPS_Text.text = "FPS : " + (1/(TimeSum / FrameCounter));
            TimeSum = 0;
            FrameCounter = 0;
        }
    }
}
