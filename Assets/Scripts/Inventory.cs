using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryItem
{
    public ItemData m_ItemData;
    public int m_Count = -1;
    private Inventory m_currentInventory;

    public InventoryItem(ItemData data, int count, Inventory currentInventory)
    {
        m_ItemData = data;
        m_Count = count;
        m_currentInventory = currentInventory;
    }

    public void Delete(int count)
    {
        m_currentInventory.RemoveItem(this, count);
    }

    public void Use()
    {
        if ((m_ItemData.m_TypeFlags & ItemType.Entertainment) == ItemType.Entertainment)
        {
            StimEntertainment.EmitStim(new StimEntertainment(10.0f));
            Delete(1);
        }
        else
        {
            Debug.Log("Does nothing!");
        }            
    }
}

public class Inventory : Singleton<Inventory> 
{    
    public delegate void OnItemUpdated(InventoryItem item);
    public static OnItemUpdated s_onItemUpdated;
    public delegate void OnItemAdded(InventoryItem item);
    public static OnItemAdded s_onItemAdded;
    public delegate void OnItemRemoved(InventoryItem item);
    public static OnItemRemoved s_onItemRemoved;

    public List<InventoryItem> m_InventoryItems = new List<InventoryItem>();
    
    void Start()
    {
        CraftPatternPlayer.s_craftSequenceEnded += OnCraftEnd;     
    }

    void OnDestroy()
    {
        CraftPatternPlayer.s_craftSequenceEnded -= OnCraftEnd;
    }

    void OnCraftEnd(ItemData itemData, CraftState state)
    {
        if(state == CraftState.Success)
        {
            itemData.m_AlreadyCrafted = true;
            AddItem(itemData, 1);
        }        
    } 

    public bool HasItem(ItemData item, int count)
    {
        for(int i = 0; i < m_InventoryItems.Count; i++)
        {
            if(m_InventoryItems[i].m_ItemData == item)
            {
                count -= m_InventoryItems[i].m_Count;
                if(count <= 0)
                    return true;
            }
        }

        return false;
    }

    void AddItem(ItemData item, int count)
    {
        InventoryItem newItem = null;

        if(item.m_IsStackable)
        {
            for(int i = 0; i < m_InventoryItems.Count; i++)
            {
                if(m_InventoryItems[i].m_ItemData == item)
                {
                    m_InventoryItems[i].m_Count += count;
                    if(s_onItemUpdated != null) s_onItemUpdated(m_InventoryItems[i]);
                    return;
                }
            }

            newItem = new InventoryItem(item, count, this);
            m_InventoryItems.Add(newItem);
        }
        else
        {
            newItem = new InventoryItem(item, count, this);
            m_InventoryItems.Add(newItem);
        }

        if(newItem != null && s_onItemAdded != null) s_onItemAdded(newItem);
    }

    public void RemoveItem(InventoryItem item, int count)
    {
        if(item.m_Count <= count)
        {
            m_InventoryItems.Remove(item);
            if(s_onItemRemoved != null) s_onItemRemoved(item);
        }
        else
        {
            item.m_Count -= count;
            if(s_onItemUpdated != null) s_onItemUpdated(item);
        }        
    }

    public void RemoveItem(ItemData item, int count)
    {
        for(int i = m_InventoryItems.Count - 1; i >= 0; i--)
        {
            if(m_InventoryItems[i].m_ItemData == item)
            {
                int countToRemove = Mathf.Min(count, m_InventoryItems[i].m_Count);
                RemoveItem(m_InventoryItems[i], countToRemove);
                count -= countToRemove;

                if(count <= 0)
                    return;
            }
        }
    }
}
