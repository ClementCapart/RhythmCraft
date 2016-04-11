using UnityEngine;
using System.Collections;

public class ContextualItemPopup : ContextualPopup 
{    
    InventoryItemUI m_item;

    public void SetData(InventoryItemUI item)
    {
        m_item = item;
        m_item.m_Button.interactable = false;
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
        m_item.m_InventoryHUDSection.SetLatestSelected(null);
    }

    public override void Close()
    {
        m_item.m_Button.interactable = true;
        base.Close();
    }
}
