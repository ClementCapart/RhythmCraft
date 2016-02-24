using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour 
{
    public GameObject m_InventoryItemsContainer = null;

    List<InventoryItemUI> m_inventoryData = new List<InventoryItemUI>();

	void Start ()
	{
	    Inventory.s_onUpdateInventory += UpdateUI;
        if (m_InventoryItemsContainer != null)
        {
            m_inventoryData = new List<InventoryItemUI>(m_InventoryItemsContainer.GetComponentsInChildren<InventoryItemUI>(true));
        }
	}

	void OnDestroy()
    {
        Inventory.s_onUpdateInventory += UpdateUI;
    }

    void UpdateUI(List<Inventory.InventoryItem> inventoryState)
    {
        for(int i = 0; i < m_inventoryData.Count; i ++)
        {
            if(inventoryState != null && inventoryState.Count > i)
            {
                m_inventoryData[i].UpdateItem(inventoryState[i]);
            }
            else
            {
                m_inventoryData[i].UpdateItem(null);
            }
        }
    }
}
