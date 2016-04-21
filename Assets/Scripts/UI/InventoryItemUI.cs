using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItemUI : MonoBehaviour 
{    
    public InventoryHUDSection m_InventoryHUDSection = null;
    public InventoryItem m_item = null;
    public bool IsEmpty
    {
        get { return m_item == null; }
    }
    
    public Image m_Icon = null;
    public Text m_NumberText = null;

    public Sprite m_MissingIconSprite = null;

    public Button m_Button = null;

	void Start () 
    {	    
        //ClearItem();
	}		

    public void OnConfirm()
    {
        ContextualPopupManager.CreateInventoryItemPopup(this);
    }

    public void ReplaceItem(InventoryItem item)
    {
        if(!IsEmpty)
        {
            ClearItem();
        }

        m_item = item;

        if (m_Icon != null)
        {
            if(m_item.m_ItemData.m_ItemIcon != null)
                m_Icon.sprite = m_item.m_ItemData.m_ItemIcon;
            else
                m_Icon.sprite = m_MissingIconSprite;
        }

        if (m_NumberText != null)
        {
            if(item.m_ItemData.m_IsStackable)
            {
                m_NumberText.gameObject.SetActive(true);
                m_NumberText.text = m_item.m_Count.ToString();
            }
            else
            {
                m_NumberText.gameObject.SetActive(false);
            }            
        }

        gameObject.SetActive(true);
    }

    public void UpdateCount()
    {
        if (m_NumberText != null)
        {
            m_NumberText.text = m_item.m_Count.ToString();
        }
    }

    public void ClearItem()
    {
        gameObject.SetActive(false);
        m_item = null;
        Destroy(gameObject);
    }

    public void Select()
    {
    }

    public void UnSelect()
    {
    }
}
