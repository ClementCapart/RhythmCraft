using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryHUDSection : HUDSection 
{
    public GameObject m_InventoryItemsContainer = null;

    List<InventoryItemUI> m_inventoryData = new List<InventoryItemUI>();

    public override void OnMaximized()
    {
        base.OnMaximized();

        for(int i = 0; i < m_inventoryData.Count; i++)
        {
            if(m_inventoryData[i].gameObject.activeInHierarchy)
                m_inventoryData[i].m_Button.interactable = true;
            else
                m_inventoryData[i].m_Button.interactable = false;
        }        

        TrySelectLatest();
    }  

    public override void OnStartMinimize()
    {      
        base.OnStartMinimize();

        m_latestSelected = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        for(int i = 0; i < m_inventoryData.Count; i++)
        {
            m_inventoryData[i].m_Button.interactable = false;
        }
    }

    public override void SubUpdate()
    {
    }

	void Awake ()
	{
	    Inventory.s_onUpdateInventory += UpdateUI;
        if (m_InventoryItemsContainer != null)
        {
            m_inventoryData = new List<InventoryItemUI>(m_InventoryItemsContainer.GetComponentsInChildren<InventoryItemUI>(true));
        }

        for(int i = 0; i < m_inventoryData.Count; i++)
        {
            m_inventoryData[i].m_InventoryHUDSection = this;
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

        //TrySelectLatest();
    }
}
