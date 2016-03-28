using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemData
{
    public bool m_Enabled = false;

    public string m_UniqueID = System.Guid.Empty.ToString();

	public string m_Name = "DefaultItemName";
    public Sprite m_ItemIcon;
    public bool m_IsStackable = true;
    public ItemType m_TypeFlags = 0x0;
    public AnimationClip m_CraftPattern = null;

    public RecipeData m_Recipe = new RecipeData();

    [System.NonSerialized]
    public bool m_AlreadyCrafted = true;
}

[System.Serializable]
public class RecipeData
{
    public List<RecipeElement> m_ItemsNeeded = new List<RecipeElement>();
}

[System.Serializable]
public class RecipeElement
{
    public string m_itemID;
    public int m_itemCount;
}

[System.Flags]
public enum ItemType
{
    Ingredient = 0x1,
    Usable = 0x2,
    Entertainment = 0x4,
}

public class ItemDatabase : AssetSingleton<ItemDatabase>
{		
	[SerializeField]
	private List<ItemData>					m_items			= new List<ItemData>();

	private	Dictionary<string, ItemData>	m_itemDatabase	= new Dictionary<string, ItemData>();

	private	void OnEnable()
	{
		foreach (ItemData item in m_items)
		{
            m_itemDatabase.Add(item.m_UniqueID, item);
		}
	}

    public void Refresh()
    {
        m_itemDatabase.Clear();
        foreach (ItemData item in m_items)
        {
            m_itemDatabase.Add(item.m_UniqueID, item);
        }
    }

    public static ItemData GetItemByUniqueID(string uniqueID)
    {
        ItemDatabase inst = Instance;
        
        if (inst != null && uniqueID != System.Guid.Empty.ToString())
        {
            return inst.GetItemByUniqueIDInternal(uniqueID);
        }

        return null;
    }

    public ItemData GetItemByUniqueIDInternal(string uniqueID)
    {
        ItemData item = null;
        
        m_itemDatabase.TryGetValue(uniqueID, out item);

        return item;
    }

	public static ItemData GetItemByName(string name)
	{
		ItemDatabase inst = Instance;

		if (inst != null && !string.IsNullOrEmpty(name))
		{
			return inst.GetItemByNameInternal(name);
		}
		return null;
	}

	public ItemData GetItemByNameInternal(string name)
	{
		foreach(KeyValuePair<string, ItemData> kvp in m_itemDatabase)
        {
            if(kvp.Value != null && kvp.Value.m_Name == name)
            {
                return kvp.Value;
            }
        }

		return null;
	}

    public static ItemData GetItemByIndex(int index)
    {
        ItemDatabase inst = Instance;

        if (inst != null && index >= 0 && index < inst.m_items.Count)
        {
            return inst.m_items[index];
        }

        return null;
    }

	public static List<ItemData> GetAllItems()
	{
        ItemDatabase inst = Instance;
		if (inst != null)
		{
            return inst.m_items;
		}

		return null;		
	}

    public static void CreateNewItem()
    {
        ItemData item = new ItemData();
        item.m_UniqueID = System.Guid.NewGuid().ToString();
        item.m_Name = "";

        if(Instance != null)
        {
            Instance.m_items.Add(item);
            Instance.m_itemDatabase.Add(item.m_UniqueID, item);
        }
    }

    public static void RemoveItem(ItemData item)
    {
        if(Instance != null)
        {
            if(Instance.m_items.Contains(item))
                Instance.m_items.Remove(item);
            if(Instance.m_itemDatabase.ContainsKey(item.m_UniqueID))
                Instance.m_itemDatabase.Remove(item.m_UniqueID);
        }
    }

    public static int GetIndexByUniqueID(string uniqueID)
    {
        if(Instance != null)
        {
            for(int i = 0; i < Instance.m_items.Count; i++)
            {
                if(Instance.m_items[i].m_UniqueID == uniqueID)
                {
                    return i;
                }
            }
        }
        
        return -1;
    }
}
