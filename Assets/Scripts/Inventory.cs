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
    
    void Start()
    {
        CraftPatternPlayer.m_craftSequenceStarted += OnCraftStart;
        CraftPatternPlayer.m_craftSequenceEnded += OnCraftEnd;
    }

    void OnDestroy()
    {
        CraftPatternPlayer.m_craftSequenceStarted -= OnCraftStart;
        CraftPatternPlayer.m_craftSequenceEnded -= OnCraftEnd;
    }

    void OnCraftStart(ItemData itemData)
    {
        for(int i = 0; i < itemData.m_Recipe.m_ItemsNeeded.Count; i++)
        {
            RemoveItem(ItemDatabase.GetItemByUniqueID(itemData.m_Recipe.m_ItemsNeeded[i].m_itemID), itemData.m_Recipe.m_ItemsNeeded[i].m_itemCount);
        }
    }

    void OnCraftEnd(ItemData itemData, CraftState state)
    {
        if(state == CraftState.Success)
        {
            AddItem(itemData, 1);
        }
    }
    

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
                    return;
                }
            }

            m_InventoryItems.Add(new InventoryItem(item, count));
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
