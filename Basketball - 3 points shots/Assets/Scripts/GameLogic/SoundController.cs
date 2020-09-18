using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource tenSecondsAudio;
    [SerializeField] private AudioSource buzzerAudio;
    [SerializeField] private AudioSource audienceClappingAudio;

    //This methods play the audio clip of the relevant audio sources.
    public void LastTenSeconds()
    {
        tenSecondsAudio.Play();
        buzzerAudio.PlayDelayed(10f);
    }
    public void ScoreThreePointsAudio()
    {
        audienceClappingAudio.Play();
    }

}
