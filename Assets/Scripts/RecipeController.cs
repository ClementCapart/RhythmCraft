using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeController : MonoBehaviour 
{
    List<ItemData> m_AvailableCrafts = new List<ItemData>();
    
    public CraftPatternPlayer m_CraftPatternPlayer = null;
    public List<Buttons> m_ButtonPerIndex = new List<Buttons>();

    Dictionary<Buttons, ItemData> m_UsableRecipes = new Dictionary<Buttons,ItemData>();
    public delegate void UsableRecipesUpdated(Dictionary<Buttons, ItemData> usableRecipes);
    public static UsableRecipesUpdated s_usableRecipesUpdated;

    void Start()
    {
        Inventory.s_onUpdateInventory += UpdateAvailableCrafts;
        UpdateAvailableCrafts(null);
    }

    void OnDestroy()
    {
        Inventory.s_onUpdateInventory -= UpdateAvailableCrafts;
    }

    void Update()
    {
        if (m_CraftPatternPlayer && !m_CraftPatternPlayer.m_IsPlaying)
        {
            foreach (KeyValuePair<Buttons, ItemData> kvp in m_UsableRecipes)
            {
                if (XInput.GetButtonDown(kvp.Key, 0))
                {
                    m_CraftPatternPlayer.StartPattern(kvp.Value);
                    break;
                }
            }
        }
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
        m_UsableRecipes.Clear();

        for (int i = 0; i < m_ButtonPerIndex.Count; i++)
        {
            if(i < m_AvailableCrafts.Count)
            {
                m_UsableRecipes.Add(GetInputPerIndex(i), m_AvailableCrafts[i]);
            }
            else
            {
                m_UsableRecipes.Add(GetInputPerIndex(i), null);
            }
        }

        if(s_usableRecipesUpdated != null) s_usableRecipesUpdated(m_UsableRecipes);
    }

    Buttons GetInputPerIndex(int index)
    {
        if(index < m_ButtonPerIndex.Count)
            return m_ButtonPerIndex[index];

        return Buttons.None;
    }
}
