using UnityEngine;
using System.Collections;

public class Entertainment : MonoBehaviour 
{
    public delegate void OnEntertainmentUpdated(float value);
    public static OnEntertainmentUpdated s_onEntertainmentUpdated;

    private float m_currentEntertainment = 0.0f;
    
    public float m_StartingEntertainment = 50.0f;

    public float m_EntertainmentDecreaseRate = 3.0f;

    void Start()
    {
        UpdateEntertainment(m_StartingEntertainment);
        StimEntertainment.SubscribeToStim(StimEntertainmentReaction);
    }

    void Update()
    {
        UpdateEntertainment(m_currentEntertainment - Time.deltaTime * m_EntertainmentDecreaseRate);
    }

    void StimEntertainmentReaction(StimEntertainment stim)
    {
        UpdateEntertainment(m_currentEntertainment + stim.m_EntertainmentValue);
    }

    void UpdateEntertainment(float newValue)
    {
        m_currentEntertainment = Mathf.Clamp(newValue, 0, 100.0f);
        if(s_onEntertainmentUpdated != null) s_onEntertainmentUpdated(m_currentEntertainment);
    }
}
