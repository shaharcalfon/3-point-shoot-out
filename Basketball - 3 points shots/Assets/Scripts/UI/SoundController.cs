using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource tenSecondsAudio;
    [SerializeField] private AudioSource buzzerAudio;
    [SerializeField] private AudioSource audienceClappingAudio;
    [SerializeField] private AudioSource NbaSound;

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
    public void PlayNbaSound()
    {
        NbaSound.Play();
    }
    public void StopNbaSound()
    {
        if(NbaSound.isPlaying)
        {
            NbaSound.Stop();
        }
        
    }
    

}
