using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntertainmentGaugeUI : MonoBehaviour 
{
    public Color m_FullColor = Color.green;
    public Color m_EmptyColor = Color.red;

    public Image m_GaugeImage = null;
    public RectTransform m_GaugeMarkerTransform = null;

    public SmoothFloat m_FillAmount = new SmoothFloat(1.0f);
    public float m_PositionLowerBound = 0.0f;
    public float m_PositionHigherBound = 900.0f;
    public float m_FillTrackingRate = 1.0f;

    void Awake()
    {
        Entertainment.s_onEntertainmentUpdated += EntertainmentUpdated;
        m_FillAmount = new SmoothFloat(m_FillTrackingRate);
        m_FillAmount.SetNow(-1.0f);
    }

    void EntertainmentUpdated(float value)
    {        
        if(m_FillAmount.Value < 0.0f)
        {
            m_FillAmount.SetNow(value / 100.0f);
        }
        else
        {
            m_FillAmount.Value = value / 100.0f;
        }
    }

    void Update()
    {
        m_FillAmount.SetTrackingRate(m_FillTrackingRate);
        m_FillAmount.Update();

        m_GaugeImage.fillAmount = m_FillAmount.Value;
        m_GaugeImage.color = Color.Lerp(m_EmptyColor, m_FullColor, m_GaugeImage.fillAmount);

        if(m_GaugeMarkerTransform)
        {
            m_GaugeMarkerTransform.anchoredPosition = new Vector2(Mathf.Clamp((1 - m_FillAmount.Value) * m_GaugeImage.rectTransform.rect.width, m_PositionLowerBound, m_PositionHigherBound), 0.0f);
        }
    }
    
}
