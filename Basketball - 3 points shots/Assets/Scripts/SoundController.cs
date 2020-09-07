using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource tenSecondsAudio;
    [SerializeField] private AudioSource buzzerAudio;
    [SerializeField] private AudioSource audienceClappingAudio;


    public void lastTenSeconds()
    {
        tenSecondsAudio.Play();
        buzzerAudio.PlayDelayed(10f);
    }
    public void scoreThreePointsAudio()
    {
        audienceClappingAudio.Play();
    }

}
