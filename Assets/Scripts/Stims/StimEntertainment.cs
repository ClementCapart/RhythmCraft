using UnityEngine;
using System.Collections;

public class StimEntertainment : StimBase<StimEntertainment> 
{
    public float m_EntertainmentValue = 10.0f;

    public StimEntertainment(float entertainmentValue)
    {
        m_EntertainmentValue = entertainmentValue;
    }
}
