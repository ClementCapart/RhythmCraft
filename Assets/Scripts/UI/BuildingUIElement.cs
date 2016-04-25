using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingUIElement : MonoBehaviour 
{
    public Sprite m_EmptyBuildingSlotIcon = null;
    public Sprite m_InitialBuildingIcon = null;
    public Image m_BuildingIcon = null;

    public Color m_NormalColor = Color.white;
    public Color m_SelectedColor = Color.white;
    public Color m_DisabledColor = Color.white;
    
    private BuildingData m_currentBuilding = null;

    void Awake()
    {
        m_BuildingIcon.sprite = m_EmptyBuildingSlotIcon;
        m_BuildingIcon.color = m_DisabledColor;
    }

    public bool IsBuilding(BuildingData building)
    {
        return m_currentBuilding == building;
    }
    
    public void SetBuilding(BuildingData building)
    {
        m_currentBuilding = building;
        if(building.m_Building != null)
        {
            m_BuildingIcon.sprite = building.m_Building.m_ItemIcon;
        }
        else
        {
            m_BuildingIcon.sprite = m_InitialBuildingIcon;
        }        
    }

    public void UpdateSelectable(bool selectable)
    {
        m_BuildingIcon.color = selectable ? m_NormalColor : m_DisabledColor;
    }

    public void Select()
    {
        m_BuildingIcon.color = m_SelectedColor;
    }

    public void Unselect()
    {
        m_BuildingIcon.color = m_NormalColor;
    }
}
