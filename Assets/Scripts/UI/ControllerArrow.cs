using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerArrow : MonoBehaviour 
{
    public Image m_ArrowImage = null;
    public Image m_FeedbackLightImage = null;

    public void Hold()
    {
        if(m_FeedbackLightImage) m_FeedbackLightImage.enabled = true;
    }

    public void Release()
    {
        if(m_FeedbackLightImage) m_FeedbackLightImage.enabled = false;
    }
}
