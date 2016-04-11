using UnityEngine;
using System.Collections;

public class ContextualItemPopup : ContextualPopup 
{    
    InventoryItemUI m_item;

    public void SetData(InventoryItemUI item)
    {
        m_item = item;
    }

    public void OnUse()
    {
        m_item.m_item.Use();
        Close();
    }

    public void OnDelete()
    {
        m_item.m_item.Delete(int.MaxValue);
        Close();        
    }
}
