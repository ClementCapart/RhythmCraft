using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftSetControllerUI : MonoBehaviour 
{
    public BuildingData.CraftSet m_CraftSet = null;
    public RectTransform m_RectTransform = null;

    public GameObject m_HiddenInPartial = null;
    public Image m_BackgroundImage = null;

    public float m_ImageAlphaInPartial = 0.3f;

    private Coroutine m_movingCoroutine = null;

    public void FullDisplay()
    {
        if(m_HiddenInPartial) m_HiddenInPartial.SetActive(true);
        if(m_BackgroundImage) m_BackgroundImage.CrossFadeAlpha(1.0f, 0.4f, false);
    }

    public void PartialDisplay()
    {
        if(m_HiddenInPartial) m_HiddenInPartial.SetActive(false);
        if(m_BackgroundImage) m_BackgroundImage.CrossFadeAlpha(m_ImageAlphaInPartial, 0.0f, false);
    }

    public void MoveTo(Vector2 position, float duration)
    {
        if(m_movingCoroutine != null)
        {
            StopCoroutine(m_movingCoroutine);
        }
        
        m_movingCoroutine = StartCoroutine(MovingTo(position, duration));
    }

    IEnumerator MovingTo(Vector2 position, float duration)
    {
        float time = 0.0f;
        Vector2 startPosition = m_RectTransform.anchoredPosition;

        while(position != m_RectTransform.anchoredPosition)
        {                        
            time += Time.deltaTime;
            if(time > duration) time = duration;
            m_RectTransform.anchoredPosition = Vector2.Lerp(startPosition, position, time / duration);            
            yield return 0;
        }
    }
}
