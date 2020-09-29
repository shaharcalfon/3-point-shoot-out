using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PowerBar : MonoBehaviour
{
    [SerializeField] private Image m_PowerBarMask;
    [SerializeField] private float m_BarChangeSpeed = 3.5f;
    private float m_MaxPowerBarValue = 100;
    private float m_CurrentPowerBarValue;
    private bool isIncreasing;
    public float FillAmount { get; private set;}
    public bool PowerBarOn;


    public void TurnOnPowerBar()
    {
        m_CurrentPowerBarValue = m_MaxPowerBarValue;        //The bar is full.
        isIncreasing = false;
        PowerBarOn = true;
        StartCoroutine(UpdatePowerBar());
    }

    //This method update the power bar according the bar change speed field.
    IEnumerator UpdatePowerBar()
    {
        while(PowerBarOn)
        {
            if(!isIncreasing)
            {
                m_CurrentPowerBarValue -= m_BarChangeSpeed;
                if(m_CurrentPowerBarValue <= 0)
                {
                    isIncreasing = true;
                }
            }
            if (isIncreasing)
            {
                m_CurrentPowerBarValue += m_BarChangeSpeed;
                if (m_CurrentPowerBarValue >= m_MaxPowerBarValue)
                {
                    isIncreasing = false;
                }
            }
            FillAmount = m_CurrentPowerBarValue / m_MaxPowerBarValue;
            m_PowerBarMask.fillAmount = FillAmount;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
        
    }

}
