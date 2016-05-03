using UnityEngine;
using System.Collections;

public class SplitCamera : MonoBehaviour 
{
    public Camera m_MainCamera = null;

    public Camera m_SecondCamera = null;

    [Range(0.0f, 1.0f)]
    public float m_SecondCameraRatio = 0.0f;

    void OnValidate()
    {
        UpdateRatio();
    }

    void UpdateRatio()
    {
        if(m_MainCamera && m_SecondCamera)
        {
            if(m_SecondCameraRatio >= 1.0f)
            {
                m_MainCamera.enabled = false;
            }
            else
            {
                m_MainCamera.enabled = true;
                SetScissorRect(m_MainCamera, new Rect(0.0f, 0.0f, 1.0f - m_SecondCameraRatio, 1.0f));
            }
            
            if(m_SecondCameraRatio <= 0.0f)
            {
                m_SecondCamera.enabled = false;
            }
            else
            {
                m_SecondCamera.enabled = true;
                SetScissorRect(m_SecondCamera, new Rect(1.0f - m_SecondCameraRatio, 0.0f, m_SecondCameraRatio, 1.0f));
            }
            
        }
    }

    public void SetScissorRect(Camera camera, Rect rect)
    {
        if(rect.x < 0.0f)
        {
            rect.width += rect.x;
            rect.x = 0.0f;
        }

        if(rect.y < 0.0f)
        {
            rect.height += rect.y;
            rect.y = 0.0f;
        }

        rect.width = Mathf.Min(1.0f - rect.x, rect.width);
        rect.height = Mathf.Min(1.0f - rect.y, rect.height);

        camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        camera.ResetProjectionMatrix();

        Matrix4x4 projectionMatrix = camera.projectionMatrix;

        camera.rect = rect;

        Matrix4x4 matrix1 = Matrix4x4.TRS(new Vector3( rect.x, rect.y, 0.0f), Quaternion.identity, new Vector3( rect.width, rect.height, 1.0f));
        Matrix4x4 matrix2 = Matrix4x4.TRS(new Vector3((1.0f / rect.width - 1.0f), (1.0f / rect.height - 1.0f), 0.0f), Quaternion.identity, new Vector3(1.0f / rect.width, 1.0f / rect.height, 1.0f));
        Matrix4x4 matrix3 = Matrix4x4.TRS(new Vector3(-rect.x * 2.0f / rect.width, - rect.y * 2.0f / rect.height, 0.0f), Quaternion.identity, Vector3.one);

        camera.projectionMatrix = matrix3 * matrix2 * projectionMatrix;
    }
}
