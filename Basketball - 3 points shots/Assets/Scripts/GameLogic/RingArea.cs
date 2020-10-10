using UnityEngine;

public class RingArea : MonoBehaviour
{
    [SerializeField] private GameObject m_ScoreArea;

    //If the player throw a ball into the ring, score area need to be active.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            m_ScoreArea.SetActive(true);
        }
    }
}
