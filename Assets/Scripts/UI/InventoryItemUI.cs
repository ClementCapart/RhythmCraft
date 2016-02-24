using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItemUI : MonoBehaviour 
{    
    Inventory.InventoryItem m_item = null;
    
    public Image m_Icon = null;
    public Text m_NumberText = null;

    public Sprite m_MissingIconSprite = null;

	void Start () 
    {	    
        UpdateItem(null);
	}		

    public void UpdateItem(Inventory.InventoryItem item)
    {
        m_item = item;

        if(item == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (m_Icon != null)
        {
            if(item.m_ItemData.m_ItemIcon != null)
                m_Icon.sprite = item.m_ItemData.m_ItemIcon;
            else
                m_Icon.sprite = m_MissingIconSprite;
        }

        if (m_NumberText != null)
        {
            if(item.m_ItemData.m_IsStackable)
            {
                m_NumberText.gameObject.SetActive(true);
                m_NumberText.text = item.m_Count.ToString();
            }
            else
            {
                m_NumberText.gameObject.SetActive(false);
            }            
        }
    }
}
