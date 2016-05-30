using UnityEngine;
using System.Collections;

public class MusicManager : Singleton<MusicManager>
{
    private bool m_PlayingMusic = false;

    public static void FadeMusicLow()
    {
        if(Instance) Instance.InternalFadeMusicLow();
    }

    public static void FadeMusicNormal()
    {
        if(Instance) Instance.InternalFadeMusicNormal();
    }

    void Start()
    {
        if(!m_PlayingMusic)
        {
            AkSoundEngine.PostEvent("Play_TavernMusic", gameObject);
            m_PlayingMusic = true;

            //For Laura's ears sake!
            FadeMusicLow();
        }
    }

    [ContextMenu("Fade Low")]
    void InternalFadeMusicLow()
    {
        AkSoundEngine.PostEvent("Set_Low_MusicBus", gameObject);
    }
    
    [ContextMenu("Fade Normal")]
    void InternalFadeMusicNormal()
    {
        AkSoundEngine.PostEvent("Set_Normal_MusicBus", gameObject);
    }
}
