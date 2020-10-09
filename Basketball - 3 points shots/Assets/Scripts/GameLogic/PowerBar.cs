using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PowerBar : MonoBehaviour
{
    private const float MaxPowerBarValue = 100;
    private const float BarChangeSpeed = 5f;

    [SerializeField] private Image m_PowerBarMask;

    private float m_CurrentPowerBarValue;
    private bool isIncreasing;
    public bool PowerBarOn;
    public float FillAmount { get; private set;}


    public void TurnOnPowerBar()
    {
        m_CurrentPowerBarValue = MaxPowerBarValue;        //The bar is full.
        isIncreasing = false;
        PowerBarOn = true;
        StartCoroutine(UpdatePowerBar());
    }

    //This method update the power bar according the bar change speed field.
    IEnumerator UpdatePowerBar()
    {
        while(PowerBarOn)
        {
            if(!isIncreasing)               //Reduce the fill
            {
                m_CurrentPowerBarValue -= BarChangeSpeed;
                if(m_CurrentPowerBarValue <= 0)
                {
                    isIncreasing = true;
                }
            }
            if (isIncreasing)               //Increase the fill
            {
                m_CurrentPowerBarValue += BarChangeSpeed;
                if (m_CurrentPowerBarValue >= MaxPowerBarValue)
                {
                    isIncreasing = false;
                }
            }
            FillAmount = m_CurrentPowerBarValue / MaxPowerBarValue;         //fill amount value is between 0 to 1.
            m_PowerBarMask.fillAmount = FillAmount;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

}
