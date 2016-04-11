using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerArrow : MonoBehaviour 
{
    public Image m_ArrowImage = null;
    public Image m_FeedbackLightImage = null;
    
    public float m_FadeInTime = 0.2f;
    public float m_FadeOutTime = 0.5f;

    public void Start()
    {
        m_FeedbackLightImage.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    public void Hold()
    {
        m_FeedbackLightImage.CrossFadeAlpha(1.0f, m_FadeInTime, false);
    }

    public void Release()
    {      
        m_FeedbackLightImage.CrossFadeAlpha(0.0f, m_FadeOutTime, false);
    }
}
