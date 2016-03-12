using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeIconUI : MonoBehaviour
{
    public Image m_Icon = null;
    public Image m_Frame = null;    

    public Sprite m_UnknownIcon = null;

	public void UpdateRecipe (ItemData itemData) 
    {
	    if(itemData == null)
        {
            if(m_Icon != null)
                m_Icon.enabled = false;
            if(m_Frame != null)
                m_Frame.enabled = false;
        }
        else
        {
            if (m_Icon != null)
            {
                if(itemData.m_AlreadyCrafted)
                    m_Icon.sprite = itemData.m_ItemIcon;
                else
                    m_Icon.sprite = m_UnknownIcon;
                m_Icon.enabled = true;
            }
            if (m_Frame != null)
            {
                m_Frame.color = Color.black;
                m_Frame.enabled = true;
            }
        }
	}
}
