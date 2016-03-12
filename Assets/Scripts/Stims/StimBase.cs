using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StimBase<StimType>
{
    private static List<Action<StimType>> s_Callbacks = new List<Action<StimType>>();

    public static void SubscribeToStim(Action<StimType> callback)
    {
        if(!s_Callbacks.Contains(callback))
            s_Callbacks.Add(callback);
        else
        {
            Debug.Log("okay");
        }
    }

    public static void UnsubscribeToStim(Action<StimType> callback)
    {
        if(s_Callbacks.Contains(callback))
        {
            s_Callbacks.Remove(callback);
        }
        else
        {
            Debug.Log("okay");
        }
    }

    public static void EmitStim(StimType stim)
    {
        for(int i = 0; i < s_Callbacks.Count; i++)
        {
            s_Callbacks[i](stim);
        }
    }
}
