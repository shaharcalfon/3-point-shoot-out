using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballSound : MonoBehaviour
{
    [SerializeField] private AudioSource m_BouncingBallAudio;
    [SerializeField] private AudioSource m_BallOnRingAudio;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            m_BouncingBallAudio.Play();
        }
        if (collision.collider.tag == "Ring") 
        {
            m_BallOnRingAudio.Play();
        }
        if (collision.collider.tag == "Glass")
        {
            m_BallOnRingAudio.Play();
        }
    }



}
