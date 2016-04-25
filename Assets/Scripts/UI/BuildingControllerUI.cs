using UnityEngine;
using System.Collections;

public class BuildingControllerUI : MonoBehaviour 
{
    public BuildingUIElement[] m_OrderedBuildingElements = new BuildingUIElement[0];
    private BuildingUIElement m_currentlySelectedUIElement = null;

    void Awake()
    {
        BuildingController.s_onBuildingSelectabilityUpdated += OnBuildingSelectabilityUpdated;
        BuildingController.s_onBuildingSelected += OnBuildingSelected;
    }

    private void OnBuildingSelected(BuildingData buildingSelected)
    {
        for(int i = 0; i < m_OrderedBuildingElements.Length; i++)
        {
            if(m_OrderedBuildingElements[i].IsBuilding(buildingSelected))
            {
                if(m_currentlySelectedUIElement != null) m_currentlySelectedUIElement.Unselect();
                m_OrderedBuildingElements[i].Select();
                m_currentlySelectedUIElement = m_OrderedBuildingElements[i];
                break;
            }
        }
    }

    private void OnBuildingSelectabilityUpdated(BuildingData buildingData, bool selectable)
    {
        BuildingUIElement firstEmpty = null;
        for(int i = 0; i < m_OrderedBuildingElements.Length; i++)
        {
            if(m_OrderedBuildingElements[i].IsBuilding(buildingData))
            {
                m_OrderedBuildingElements[i].UpdateSelectable(selectable);
                firstEmpty = null;
                break;
            }
            else if(selectable && firstEmpty == null && m_OrderedBuildingElements[i].IsBuilding(null))
            {
                firstEmpty = m_OrderedBuildingElements[i];
            }
        }

        if(selectable && firstEmpty != null)
        {
            firstEmpty.SetBuilding(buildingData);
            firstEmpty.UpdateSelectable(selectable);
        }

 	    Debug.Log((buildingData.m_Building != null ? buildingData.m_Building.m_Name : "Initial Building") + (selectable ? " Added" : " Removed"));
    }

}
