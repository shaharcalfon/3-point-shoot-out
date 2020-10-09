using UnityEngine;

public class scoreArea : MonoBehaviour
{
    [SerializeField] gameController m_gameController;
    [SerializeField] UIContoller m_UIController;
    [SerializeField] SoundController m_SoundController;

    //This method invoke when the player is made score.
    private void OnTriggerEnter(Collider other)         
    {
        if(other.gameObject.tag=="Ball")
        {
            m_gameController.AddThreePoints();
            m_UIController.UpdateScoreText();
            m_SoundController.ScoreThreePointsAudio();
        }
    }
}
