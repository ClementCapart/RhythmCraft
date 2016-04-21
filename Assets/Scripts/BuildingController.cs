using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour 
{
    Dictionary<BuildingData, int> m_AvailableBuildings = null;

    void Awake()
    {
        Inventory.s_onItemAdded += CheckIfBuildingAdded;
        Inventory.s_onItemRemoved += CheckIfBuildingRemoved;
    }

    void OnEnable()
    {
        if(m_AvailableBuildings == null) m_AvailableBuildings = new Dictionary<BuildingData,int>();
    }

    void Destroy()
    {
        Inventory.s_onItemAdded -= CheckIfBuildingAdded;
        Inventory.s_onItemRemoved -= CheckIfBuildingRemoved;
    }

    void Update()
    {
        Debug.Log(m_AvailableBuildings.Count);
    }

    void CheckIfBuildingAdded(InventoryItem item)
    {
        if((item.m_ItemData.m_TypeFlags & ItemType.Building) != 0)
        {
            BuildingData data = ItemDatabase.GetBuildingByUniqueID(item.m_ItemData.m_UniqueID);
            if(m_AvailableBuildings.ContainsKey(data))
            {
                m_AvailableBuildings[data]++;
            }
            else
            {
                m_AvailableBuildings.Add(data, 1);
            }            
        }
    }

    void CheckIfBuildingRemoved(InventoryItem item)
    {
        if((item.m_ItemData.m_TypeFlags & ItemType.Building) != 0)
        {   
            BuildingData data = ItemDatabase.GetBuildingByUniqueID(item.m_ItemData.m_UniqueID);
            if(m_AvailableBuildings.ContainsKey(data))
            {
                int value = --m_AvailableBuildings[data];
                if(value <= 0)
                {
                    m_AvailableBuildings.Remove(data);
                }
            }
        }
    }
}
