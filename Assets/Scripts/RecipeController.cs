using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeController : MonoBehaviour 
{
    List<ItemData> m_AvailableCrafts = new List<ItemData>();
    
    public CraftPatternPlayer m_CraftPatternPlayer = null;

    public BuildingData m_CurrentBuilding = null;
    public BuildingData.CraftSet m_CurrentCraftSet = null;
    private int m_craftSetIndex = -1;

    public delegate void OnCraftSetSelected(BuildingData.CraftSet craftSet);
    
    public static OnCraftSetSelected s_OnCraftSetSelected = null;

    void Start()
    {
        BuildingController.s_onBuildingSelected += OnBuildingChanged;
    }
   
    void OnBuildingChanged(BuildingData newBuilding)
    {
        m_CurrentBuilding = newBuilding;        

        if(m_CurrentBuilding != null)
        {
            m_CurrentCraftSet = m_CurrentBuilding.GetCraftSet();
            m_craftSetIndex = 0;
            if(m_CurrentCraftSet != null && s_OnCraftSetSelected != null) s_OnCraftSetSelected(m_CurrentCraftSet);
        }
    }

    void OnDestroy()
    {
    }

    void Update()
    {        
        if (m_CraftPatternPlayer && m_CraftPatternPlayer.m_State == CraftPatternPlayer.PlayerState.Stopped && !HUDSectionSelection.HasSelection())
        {
            if(XInput.GetButtonUp(Buttons.RightBumper, 0))
            {
                SelectNextCraftSet();
            }

            if(m_CurrentCraftSet != null)
            {
                foreach(KeyValuePair<Buttons, ItemData> kvp in m_CurrentCraftSet.GetRecipes())
                {
                    if(XInput.GetButtonDown(kvp.Key, 0))
                    {
                        m_CraftPatternPlayer.StartPattern(kvp.Value);
                        break;
                    }
                }
            }            
        }
    }

    void SelectNextCraftSet()
    {
        m_craftSetIndex = (m_craftSetIndex + 1) % m_CurrentBuilding.m_CraftSets.Count;
        m_CurrentCraftSet = m_CurrentBuilding.GetCraftSet(m_craftSetIndex);
        if(m_CurrentCraftSet != null && s_OnCraftSetSelected != null) s_OnCraftSetSelected(m_CurrentCraftSet);
    }        
}
