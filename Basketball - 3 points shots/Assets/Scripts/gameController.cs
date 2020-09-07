using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    [SerializeField] private AudioSource threeSecondsAudio;
    [SerializeField] private AudioSource buzzerAudio;
    public int numberOfShotsThrown = 0;

    private void lastThreeSeconds()
    {
        threeSecondsAudio.Play();
        buzzerAudio.PlayDelayed(2.8f);
    }
    void Start()
    {
       // lastThreeSeconds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
