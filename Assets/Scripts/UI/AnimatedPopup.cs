using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedPopup : MonoBehaviour 
{
    public float m_LifeTime = 3.0f;
    public Image m_Image;
    public Animator m_Animator;
    public RectTransform m_RectTransform;

    public void Initialize(Sprite sprite, string animationToPlay)
    {
        m_Image.sprite = sprite;
        m_Animator.Play(animationToPlay);
    }

    public void SetPosition(Vector3 position)
    {
        m_RectTransform.anchoredPosition = position;
    }

    public void Update()
    {
        m_LifeTime -= Time.deltaTime;
        if(m_LifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
