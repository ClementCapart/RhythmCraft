using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntertainmentGaugeUI : MonoBehaviour 
{
    public Color m_FullColor = Color.green;
    public Color m_EmptyColor = Color.red;

    public Image m_GaugeImage = null;

    void Awake()
    {
        Entertainment.s_onEntertainmentUpdated += EntertainmentUpdated;
    }

    void EntertainmentUpdated(float value)
    {
        m_GaugeImage.fillAmount = value / 100.0f;
        m_GaugeImage.color = Color.Lerp(m_EmptyColor, m_FullColor, m_GaugeImage.fillAmount);
    }
    
}
