using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryHUDSection : HUDSection 
{
    public GameObject m_InventoryItemPrefab = null;

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
	    Inventory.s_onItemUpdated += UpdateUIItem;
        Inventory.s_onItemAdded += AddUIItem;
        Inventory.s_onItemRemoved += RemoveUIItem;
	}

	void OnDestroy()
    {
        Inventory.s_onItemUpdated -= UpdateUIItem;
        Inventory.s_onItemAdded -= AddUIItem;
        Inventory.s_onItemRemoved -= RemoveUIItem;
    }

    public override void TrySelectLatest()
    {
        if (m_latestSelected && m_latestSelected.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(m_latestSelected);
        }
        else
        {
            if(m_inventoryData.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(m_inventoryData[0].gameObject);
            }
        }
    }

    void UpdateUIItem(InventoryItem item)
    {
        for(int i = 0; i < m_inventoryData.Count; i ++)
        {
            if(m_inventoryData[i].m_item == item)
            {
                m_inventoryData[i].UpdateCount();
                return;
            }
        }        
    }

    void AddUIItem(InventoryItem item)
    {
        GameObject obj = Instantiate(m_InventoryItemPrefab) as GameObject;
        obj.transform.SetParent(m_InventoryItemsContainer.transform, false);

        InventoryItemUI inventoryItem = obj.GetComponent<InventoryItemUI>();
        if (inventoryItem)
        {
            inventoryItem.m_InventoryHUDSection = this;
            inventoryItem.ReplaceItem(item);
            m_inventoryData.Add(inventoryItem);
        }
    }

    void RemoveUIItem(InventoryItem item)
    {
        for(int i = m_inventoryData.Count - 1; i >= 0; i--)
        {
            if(m_inventoryData[i].m_item == item)
            {
                m_inventoryData[i].ClearItem();
                m_inventoryData.RemoveAt(i);
                return;
            }
        }
    }
}
