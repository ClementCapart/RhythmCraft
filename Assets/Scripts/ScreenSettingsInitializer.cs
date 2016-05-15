using UnityEngine;
using System.Collections;

public class ScreenSettingsInitializer : MonoBehaviour 
{
    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }
}
