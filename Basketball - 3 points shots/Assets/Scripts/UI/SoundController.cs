using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource m_TenSecondsAudio;
    [SerializeField] private AudioSource m_BuzzerAudio;
    [SerializeField] private AudioSource m_AudienceClappingAudio;
    [SerializeField] private AudioSource m_NbaSound;

    //This methods play the audio clip of the relevant audio sources.
    public void LastTenSeconds()
    {
        m_TenSecondsAudio.Play();
        m_BuzzerAudio.PlayDelayed(10f);
    }
    public void ScoreThreePointsAudio()
    {
        m_AudienceClappingAudio.Play();
    }
    public void PlayNbaSound()
    {
        m_NbaSound.Play();
    }
    public void StopSound()
    {
        if(m_NbaSound.isPlaying)
        {
            m_NbaSound.Stop();
        }
        if (m_TenSecondsAudio.isPlaying)
        {
            m_TenSecondsAudio.Stop();
            m_BuzzerAudio.Stop();
        }
    }
}
