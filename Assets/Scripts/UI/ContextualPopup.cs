using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ContextualPopup : MonoBehaviour 
{
    public RectTransform m_RectTransform = null;
    public HUDSection    m_ContainerSection = null;

    public GameObject    m_InitialFocusObject = null;

    void OnEnable()
    {
    }

    void OnDestroy()
    {
        if(m_ContainerSection)
        {
            m_ContainerSection.OnChangeState -= OnSectionChangeState;
            m_ContainerSection.TrySelectLatest();
        }                
    }

    public void Initialize(HUDSection containerSection)
    {
        if(containerSection)
        {
            m_ContainerSection = containerSection;
            m_ContainerSection.OnChangeState += OnSectionChangeState;
            m_ContainerSection.SetLatestSelected(EventSystem.current.currentSelectedGameObject);
        }

        EventSystem.current.SetSelectedGameObject(m_InitialFocusObject);
    }

    void Update()
    {
    }

    public virtual void Close()
    {
        Destroy(gameObject);
    }

    void OnSectionChangeState(HUDSectionState state)
    {
        if(state == HUDSectionState.Minimizing)
        {
            Destroy(gameObject);
        }
    }

    public void SetPosition(RectTransform rectTransform)
    {
        if(m_RectTransform)
        {
            m_RectTransform.position = rectTransform.position;
        }
    }
}
