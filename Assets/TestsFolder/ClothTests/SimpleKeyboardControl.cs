using UnityEngine;
using System.Collections;

public class SimpleKeyboardControl : MonoBehaviour
{

    void Update()
    {
        float speed = 5.0f;
        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            speed = 1.0f;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized * speed * Time.deltaTime * -1.0f;
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized * speed * Time.deltaTime * -1.0f;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized * speed * Time.deltaTime;
        }
    }
}
