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

    public delegate void OnBuildingSelected(BuildingData building);
    public delegate void OnCraftSetSelected(BuildingData.CraftSet craftSet);
    
    public static OnBuildingSelected s_OnBuildingSelected = null;
    public static OnCraftSetSelected s_OnCraftSetSelected = null;

    void Start()
    {
        Inventory.s_onUpdateInventory += UpdateAvailableCrafts;
        OnBuildingChanged(ItemDatabase.GetBuildingByUniqueID(""));        
        UpdateAvailableCrafts(null);
    }
   
    void OnBuildingChanged(BuildingData newBuilding)
    {
        m_CurrentBuilding = newBuilding;        

        if(m_CurrentBuilding != null)
        {
            if(s_OnBuildingSelected != null) s_OnBuildingSelected(m_CurrentBuilding);

            m_CurrentCraftSet = m_CurrentBuilding.GetCraftSet();
            m_craftSetIndex = 0;
            if(m_CurrentCraftSet != null && s_OnCraftSetSelected != null) s_OnCraftSetSelected(m_CurrentCraftSet);
        }
    }

    void OnDestroy()
    {
        Inventory.s_onUpdateInventory -= UpdateAvailableCrafts;
    }

    void Update()
    {        
        if (m_CraftPatternPlayer && m_CraftPatternPlayer.m_State == CraftPatternPlayer.PlayerState.Stopped && !HUDSectionSelection.HasSelection())
        {
            if(XInput.GetButtonUp(Buttons.RightBumper, 0))
            {
                SelectNextCraftSet();
            }
            /*foreach (KeyValuePair<Buttons, ItemData> kvp in m_UsableRecipes)
            {
                if (XInput.GetButtonDown(kvp.Key, 0))
                {
                    m_CraftPatternPlayer.StartPattern(kvp.Value);
                    break;
                }
            }*/
        }
    }

    void SelectNextCraftSet()
    {
        m_craftSetIndex = (m_craftSetIndex + 1) % m_CurrentBuilding.m_CraftSets.Count;
        m_CurrentCraftSet = m_CurrentBuilding.GetCraftSet(m_craftSetIndex);
        if(m_CurrentCraftSet != null && s_OnCraftSetSelected != null) s_OnCraftSetSelected(m_CurrentCraftSet);
    }

    void UpdateAvailableCrafts(List<Inventory.InventoryItem> inventoryData)
    {
        m_AvailableCrafts.Clear();

        List<ItemData> itemData = ItemDatabase.GetAllItems();

        for(int i = 0; i < itemData.Count; i++)
        {
            if(!itemData[i].m_Enabled)
                continue;

            if(itemData[i].m_Recipe.m_ItemsNeeded != null && itemData[i].m_Recipe.m_ItemsNeeded.Count > 0)
            {
                if(inventoryData == null || inventoryData.Count == 0)
                    continue;

                bool missingIngredients = false;

                foreach(RecipeElement element in itemData[i].m_Recipe.m_ItemsNeeded)
                {
                    int countNeeded = element.m_itemCount;

                    for(int j = 0; j < inventoryData.Count; j++)
                    {
                        if(inventoryData[j].m_ItemData.m_UniqueID == element.m_itemID)
                        {
                            if(countNeeded < inventoryData[j].m_Count)
                            {
                                countNeeded = 0;
                                break;
                            }
                            else
                            {
                                countNeeded -= inventoryData[j].m_Count;
                            }
                        }
                    }

                    if(countNeeded > 0)
                    {
                        missingIngredients = true;
                        break;
                    }
                }

                if(!missingIngredients)
                {
                    m_AvailableCrafts.Add(itemData[i]);
                }
            }
            else
            {
                m_AvailableCrafts.Add(itemData[i]);
            }
        }

        UpdateRecipeInput();
    }

    void UpdateRecipeInput()
    {            
        //if(s_usableRecipesUpdated != null) s_usableRecipesUpdated(m_UsableRecipes);
    }    
}
