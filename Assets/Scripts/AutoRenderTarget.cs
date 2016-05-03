using UnityEngine;
using System.Collections;

public class AutoRenderTarget : MonoBehaviour 
{
    public RenderTexture m_Texture;
    public Camera m_Camera;

    void Awake()
    {
        if(m_Camera == null)
        {
            m_Camera = GetComponent<Camera>();
        }

        if(m_Texture == null)
        {
            m_Texture = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            m_Texture.Create();
        }

        m_Camera.targetTexture = m_Texture;
    }
}
