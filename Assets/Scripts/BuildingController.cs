using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingController : Controller 
{
    public enum ControlDirection
    {
        Horizontal,
        Vertical,
    }

    public class ListedBuilding
    {
        public BuildingData m_Building = null;
        public bool m_Selectable = false;
    }

    public ControlDirection m_ControlDirection = ControlDirection.Vertical;
    public bool m_CycleThrough = false;
    public bool m_InvertedDirection = true;

    Dictionary<BuildingData, int> m_AvailableBuildings = null;

    List<ListedBuilding> m_OrderedBuildings = new List<ListedBuilding>();
    BuildingData m_currentBuildingSelected = null;   
    int m_currentIndex = -1;

    public delegate void OnBuildingSelected(BuildingData buildingSelected);
    public static OnBuildingSelected s_onBuildingSelected;

    public delegate void OnBuildingSelectabilityUpdated(BuildingData buildingData, bool selectable);
    public static OnBuildingSelectabilityUpdated s_onBuildingSelectabilityUpdated;

    void Awake()
    {
        Controller.RegisterController(this);
        Inventory.s_onItemAdded += CheckIfBuildingAdded;
        Inventory.s_onItemRemoved += CheckIfBuildingRemoved;
    }

    void Start()
    {
        if (m_AvailableBuildings == null) m_AvailableBuildings = new Dictionary<BuildingData, int>();
        else m_AvailableBuildings.Clear();

        AddInitialBuilding();
        SelectBuilding(+1);
    }

    void AddInitialBuilding()
    {
        ItemData initialBuilding = ItemDatabase.GetItemByName("Carpentry");
        if (initialBuilding != null)
        {
            BuildingData buildingData = ItemDatabase.GetBuildingByUniqueID(initialBuilding.m_UniqueID);
            if (buildingData != null)
            {
                m_AvailableBuildings.Add(buildingData, 1);
                AddBuilding(buildingData);
            }
        }
    }

    void Destroy()
    {
        Inventory.s_onItemAdded -= CheckIfBuildingAdded;
        Inventory.s_onItemRemoved -= CheckIfBuildingRemoved;
    }

    void OnDestroy()
    {
        Controller.UnregisterController(this);
    }

    void Update()
    {
        if(m_AvailableBuildings.Count <= 1 || HUDSectionSelection.HasSelection() || IsLocked())
            return;

        switch(m_ControlDirection)
        {
            case ControlDirection.Horizontal:
                if(XInput.GetButtonDown(Buttons.Left, 0))
                {
                    SelectBuilding(!m_InvertedDirection ? -1 : +1);
                }
                else if(XInput.GetButtonDown(Buttons.Right, 0))
                {
                    SelectBuilding(!m_InvertedDirection ? +1 : -1);
                }
                break;

            case ControlDirection.Vertical:
                if(XInput.GetButtonDown(Buttons.Down, 0))
                {
                    SelectBuilding(!m_InvertedDirection ? -1 : +1);
                }
                else if(XInput.GetButtonDown(Buttons.Up, 0))
                {
                    SelectBuilding(!m_InvertedDirection ? +1 : -1);
                }
                break;
        }
    }

    void SelectBuilding(int direction, bool forceCycle = false)
    {
        int maxLoopCount = 100;
        int initialIndex = m_currentIndex;

        m_currentIndex += direction;        

        bool done = false;

        while(!done && maxLoopCount > 0)
        {
            if(m_currentIndex < 0)
            {
                if(m_CycleThrough || forceCycle) m_currentIndex = m_OrderedBuildings.Count - 1;
                else m_currentIndex = initialIndex;
            }
            else if(m_currentIndex >= m_OrderedBuildings.Count)
            {
                if(m_CycleThrough || forceCycle) m_currentIndex = 0;
                else m_currentIndex = initialIndex;
            }

            if(m_OrderedBuildings[m_currentIndex].m_Selectable)
            {
                m_currentBuildingSelected = m_OrderedBuildings[m_currentIndex].m_Building;
                done = true;
            }
            else
            {
                m_currentIndex += direction;
            }

            maxLoopCount--;
            if(maxLoopCount < 0)
            {
                Debug.LogError("Max Loop Count reached in BuildingController::SelectBuilding( " + direction + " ). Should never happen.");
            }
        }

        if(s_onBuildingSelected != null) s_onBuildingSelected(m_currentBuildingSelected);        
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
                AddBuilding(data);
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
                    RemoveBuilding(data);
                }
            }
        }
    }

    void AddBuilding(BuildingData building)
    {
        bool found = false;
        for(int i = 0; i < m_OrderedBuildings.Count; i ++)
        {
            if(m_OrderedBuildings[i].m_Building == building)
            {
                found = true;
                m_OrderedBuildings[i].m_Selectable = true;
                break;
            }
        }

        if(!found)
        {
            ListedBuilding listedBuilding = new ListedBuilding();
            listedBuilding.m_Building = building;
            listedBuilding.m_Selectable = true;
            m_OrderedBuildings.Add(listedBuilding);
        }        

        if(s_onBuildingSelectabilityUpdated != null) s_onBuildingSelectabilityUpdated(building, true);
    }

    void RemoveBuilding(BuildingData building)
    {
        for (int i = 0; i < m_OrderedBuildings.Count; i++)
        {
            if (m_OrderedBuildings[i].m_Building == building)
            {
                if(m_currentBuildingSelected == building)
                {
                    SelectBuilding(-1, true);
                }
                m_OrderedBuildings[i].m_Selectable = false;
                if(s_onBuildingSelectabilityUpdated != null) s_onBuildingSelectabilityUpdated(building, false);
                
                break;
            }
        }
    }
}
