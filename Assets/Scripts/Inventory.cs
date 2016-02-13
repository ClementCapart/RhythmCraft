using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
    public class InventoryItem
    {
        public ItemData m_ItemData;
        public int m_Count = -1;

        public InventoryItem(ItemData data, int count)
        {
            m_ItemData = data;
            m_Count = count;
        }
    }

    public List<InventoryItem> m_InventoryItems = new List<InventoryItem>();

    bool HasItem(ItemData item, int count)
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
        if(item.m_IsStackable)
        {
            for(int i = 0; i < m_InventoryItems.Count; i++)
            {
                if(m_InventoryItems[i].m_ItemData == item)
                {
                    m_InventoryItems[i].m_Count += count;
                }
            }
        }
        else
        {
            m_InventoryItems.Add(new InventoryItem(item, count));
        }
    }

    void RemoveItem(ItemData item, int count)
    {
        for(int i = m_InventoryItems.Count - 1; i >= 0; i--)
        {
            if(m_InventoryItems[i].m_ItemData == item)
            {
                if(m_InventoryItems[i].m_Count > count)
                {
                    m_InventoryItems[i].m_Count -= count;
                    return;
                }
                else
                {
                    count -= m_InventoryItems[i].m_Count;
                    m_InventoryItems.RemoveAt(i);
                }

                if(count <= 0)
                    return;
            }
        }
    }
}
