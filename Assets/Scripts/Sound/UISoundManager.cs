using UnityEngine;
using System.Collections;

public class UISoundManager : Singleton<UISoundManager> 
{
    public static void PlaySound(string eventName)
    {
        if(Instance)
        {
            AkSoundEngine.PostEvent(eventName, Instance.gameObject);
        }
    }

    void Awake()
    {
        CraftPatternPlayer.s_craftSequenceEnded += OnCraftSequenceEnded;
    }
    void Destroy()
    {
        CraftPatternPlayer.s_craftSequenceEnded -= OnCraftSequenceEnded;
    }

    private void OnCraftSequenceEnded(ItemData item, CraftState state)
    {
        switch(state)
        {
            case CraftState.Success:
                PlaySound("Play_SuccessCraft");
                break;

            case CraftState.NearSuccess:
            case CraftState.Failure:
                PlaySound("Play_FailCraft");
                break;
        }
    }

    


}
