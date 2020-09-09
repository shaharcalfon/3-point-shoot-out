using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerBar : MonoBehaviour
{
    [SerializeField] private Image m_PowerBarMask;
    [SerializeField] private float m_BarChangeSpeed = 1.8f;
    private float m_MaxPowerBarValue = 100;
    private float m_CurrentPowerBarValue;
    private bool isIncreasing;
    public float m_FillAmount;
    public bool PowerBarOn;


    public void TurnOnPowerBar()
    {
        m_CurrentPowerBarValue = m_MaxPowerBarValue;
        isIncreasing = false;
        PowerBarOn = true;
        StartCoroutine(UpdatePowerBar());
    }
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
            m_FillAmount = m_CurrentPowerBarValue / m_MaxPowerBarValue;
            m_PowerBarMask.fillAmount = m_FillAmount;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
        
    }

}
