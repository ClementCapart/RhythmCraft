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
                m_Icon.sprite = m_UnknownIcon;
            if(m_Frame != null)
                m_Frame.color = Color.red;
        }
        else
        {
            if(m_Icon != null)
                m_Icon.sprite = itemData.m_ItemIcon;
            if(m_Frame != null)
                m_Frame.color = Color.black;
        }
	}
}
