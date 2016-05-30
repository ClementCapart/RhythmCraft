using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechUIElement : MonoBehaviour 
{
    public RectTransform m_RectTransform = null;

    public Image m_CharacterImage = null;
    public Text m_BubbleText = null;

    public enum State
    {
        NotStarted,
        StartDisplay,
        TextDisplayed,
        RequestNewText,
        Disappeared,
    }

    private State m_state = State.NotStarted;
    public State GetState()
    {
        return m_state;
    }

    public void DisplayText(string text, float displaySpeed = 0.0f)
    {
        m_state = State.StartDisplay;
        if(displaySpeed <= 0)
        {
            m_BubbleText.text = text;
            m_state = State.TextDisplayed;
        }
        else
        {
            StartCoroutine(DisplayTextCoroutine(text, displaySpeed));
        }        
    }

    IEnumerator DisplayTextCoroutine(string text,  float displaySpeed)
    {
        m_state = State.RequestNewText;
        m_BubbleText.text = "";

        float currentCharacter = 0;
        
        while(currentCharacter < text.Length)
        {
            currentCharacter = Mathf.Min(currentCharacter + displaySpeed * Time.deltaTime, text.Length);
            m_BubbleText.text = text.Substring(0, Mathf.FloorToInt(currentCharacter));
            yield return 0;
        }        

        m_state = State.TextDisplayed;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
