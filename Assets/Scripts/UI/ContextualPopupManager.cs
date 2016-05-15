using UnityEngine;
using System.Collections;

public class ContextualPopupManager : Singleton<ContextualPopupManager> 
{
    public GameObject m_InventoryItemPopupPrefab = null;
    public GameObject m_RecipeInfoPopupPrefab = null;

    public GameObject m_ItemCraftEndPopupPrefab = null;
    public Transform m_ItemCraftEndPopupLocation = null;

    private ContextualPopup m_currentPopup = null;

    public static void CreateInventoryItemPopup(InventoryItemUI item)
    {
        if(item != null && Instance)
        {
            GameObject popup = Instantiate<GameObject>(Instance.m_InventoryItemPopupPrefab);
            popup.transform.SetParent(Instance.transform);  
            ContextualItemPopup itemPopup = popup.GetComponent<ContextualItemPopup>();
            if(Instance.m_currentPopup != null)
            {
                Instance.m_currentPopup.Close();
            }

            Instance.m_currentPopup = itemPopup;
            itemPopup.SetPosition(item.GetComponent<RectTransform>());
            itemPopup.Initialize(item.m_InventoryHUDSection);
            itemPopup.SetData(item);
        }
    }

    public static void CreateRecipeInfoPopup(ButtonRecipeUI buttonRecipe, HUDSection hudSection)
    {
        if(buttonRecipe != null && Instance)
        {
            GameObject popup = Instantiate<GameObject>(Instance.m_RecipeInfoPopupPrefab);
            popup.transform.SetParent(Instance.transform);

            RecipeInfoPopup itemPopup = popup.GetComponent<RecipeInfoPopup>();
            if(Instance.m_currentPopup != null)
            {
                Instance.m_currentPopup.Close();
            }

            Instance.m_currentPopup = itemPopup;
            itemPopup.SetPosition(buttonRecipe.GetComponent<RectTransform>());
            itemPopup.Initialize(hudSection);
            itemPopup.SetData(buttonRecipe.Item);
        }
    }

    public static void CreateCraftResultPopup(ItemData itemData, CraftState endState)
    {
        if (Instance)
        {
            GameObject popup = Instantiate<GameObject>(Instance.m_ItemCraftEndPopupPrefab);
            popup.transform.SetParent(Instance.m_ItemCraftEndPopupLocation, false);

            AnimatedPopup animatedPopup = popup.GetComponent<AnimatedPopup>();
            if (animatedPopup != null)
            {
                if (endState == CraftState.Success)
                {
                    animatedPopup.Initialize(itemData.m_ItemIcon, "SuccessCraft");
                }
                else
                {
                    if (itemData.m_FailedBaseIcon != null && (itemData.m_TypeFlags & ItemType.Base) != 0)
                    {
                        animatedPopup.Initialize(itemData.m_FailedBaseIcon, "FailCraft");                        
                    }
                    else
                    {
                        animatedPopup.Initialize(itemData.m_ItemIcon, "FailCraft");
                    }
                }
            }
        }
    }        
}
