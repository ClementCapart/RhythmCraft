using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryHUDSection : HUDSection 
{
    public GameObject m_InventoryItemsContainer = null;

    List<InventoryItemUI> m_inventoryData = new List<InventoryItemUI>();

    private GameObject m_latestSelected = null;

    public override void OnMaximized()
    {
        base.OnMaximized();

        for(int i = 0; i < m_inventoryData.Count; i++)
        {
            m_inventoryData[i].m_Button.interactable = true;
        }        

        if(m_latestSelected) EventSystem.current.SetSelectedGameObject(m_latestSelected);
        else EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
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
