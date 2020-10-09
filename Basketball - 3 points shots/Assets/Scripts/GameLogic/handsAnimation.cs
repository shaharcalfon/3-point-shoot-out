using UnityEngine;

public class handsAnimation : MonoBehaviour
{
    [SerializeField] private playerController m_playerController;
    public void SetBallParent()
    {
        m_playerController.SetBallParent();
    }
    public void SetBallPosition()
    {
        m_playerController.SetBallPosition();
    }
}
