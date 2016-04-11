using UnityEngine;
using System.Collections;

public class ContextualPopupManager : Singleton<ContextualPopupManager> 
{
    public GameObject m_InventoryItemPopupPrefab = null;

    private ContextualPopup m_currentPopup = null;

    public static void CreateInventoryItemPopup(InventoryItemUI item)
    {
        if(item != null && Instance)
        {
            GameObject popup = Instantiate<GameObject>(Instance.m_InventoryItemPopupPrefab);
            popup.transform.SetParent(Instance.transform);  
            ContextualItemPopup itemPopup = popup.GetComponent<ContextualItemPopup>();
            Instance.m_currentPopup = itemPopup;
            itemPopup.SetPosition(item.GetComponent<RectTransform>());
            itemPopup.Initialize(item.m_InventoryHUDSection);
            itemPopup.SetData(item);
        }
    }
}
