using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Ball")
        {
            FindObjectOfType<UIContoller>().AddThreePoints();
            FindObjectOfType<SoundController>().ScoreThreePointsAudio();
        }
    }
}
