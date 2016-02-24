using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeController : MonoBehaviour 
{
    List<ItemData> m_AvailableCrafts = new List<ItemData>();

    ItemData[] m_UsableRecipes = new ItemData[4];

    void Start()
    {
        Inventory.s_onUpdateInventory += UpdateAvailableCrafts;
        UpdateAvailableCrafts(null);
    }

    void OnDestroy()
    {
        Inventory.s_onUpdateInventory -= UpdateAvailableCrafts;
    }

    void UpdateAvailableCrafts(List<Inventory.InventoryItem> inventoryData)
    {
        m_AvailableCrafts.Clear();

        List<ItemData> itemData = ItemDatabase.GetAllItems();

        for(int i = 0; i < itemData.Count; i++)
        {
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
    }
}
