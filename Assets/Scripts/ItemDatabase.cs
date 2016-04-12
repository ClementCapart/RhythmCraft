using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class BuildingData
{
    public class CraftSet
    {
        private Dictionary<Buttons, ItemData> m_AssignedRecipes = null;

        public CraftSet()
        {
            m_AssignedRecipes = new Dictionary<Buttons,ItemData>();
        }

        public int GetCount()
        {
            return m_AssignedRecipes.Count;
        }

        public void AddAssignedRecipe(Buttons button, ItemData item)
        {
            m_AssignedRecipes.Add(button, item);
        }
    }

    public ItemData m_Building = null;
    public List<ItemData> m_Recipes = null;
    public List<CraftSet> m_CraftSets = null;

    public BuildingData(ItemData building)
    {
        m_Building = building;
    }

    public void AddRecipe(ItemData itemData)
    {
        if(m_Recipes == null)
        {
            m_Recipes = new List<ItemData>();
        }

        m_Recipes.Add(itemData);
    }

    public CraftSet GetCraftSet(int index = 0)
    {
        if(m_CraftSets != null && m_CraftSets.Count > index)
        {
            return m_CraftSets[index];
        }
        
        return null;
    }

    public void AssignButtons(List<Buttons> buttonPerIndex)
    {
        if(m_Recipes.Count == 0)
            return;

        m_CraftSets = new List<CraftSet>();

        int currentButtonIndex = 0;
        CraftSet currentCraftSet = new CraftSet();
        m_CraftSets.Add(currentCraftSet);        

        foreach (ItemData item in m_Recipes)
        {
            if(currentButtonIndex >= buttonPerIndex.Count)
            {
                currentButtonIndex = 0;
                currentCraftSet = new CraftSet();
                m_CraftSets.Add(currentCraftSet);
            }

            if(item.m_Enabled)
            {
                currentCraftSet.AddAssignedRecipe(buttonPerIndex[currentButtonIndex], item);
                currentButtonIndex++;             
            }
        }
    }
}

[System.Serializable]
public class CraftPattern
{
    [System.Serializable]
    public class PatternNote
    {
        public Direction m_Direction = Direction.None;
        public float m_Delay = 0.0f;

        public string m_Serialized = "";

        public PatternNote(string serialized)
        {
            Unserialize(serialized);
        }

        public void Unserialize(string serialized)
        {
            m_Serialized = serialized;

            switch(m_Serialized[0])
            {
                case '#':
                    m_Direction = Direction.None;
                    break;

                case 'U':
                    m_Direction = Direction.Up;
                    break;

                case 'R':
                    m_Direction = Direction.Right;
                    break;

                case 'D':
                    m_Direction = Direction.Down;
                    break;

                case 'L':
                    m_Direction = Direction.Left;
                    break;

                default:
                    Debug.LogError("CraftNote Parsing: Direction input is wrong, check database.");
                    break;
            }

            string delayString = m_Serialized.Remove(0, 1);

            if(!float.TryParse(delayString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out m_Delay))
            {
                Debug.LogError("Craft Note Parsing: Delay input is wrong, check database.");
            }
        }

        public override string ToString()
        {
            return m_Direction.ToString() + ":" + m_Delay.ToString();
        }
    }

    public string m_Serialized = "";
    private PatternNote[] m_Pattern = null;

    public CraftPattern(string serialized)
    {
        Unserialize(serialized);
    }

    public PatternNote[] GetPattern()
    {
        return m_Pattern;
    }

    public void Unserialize(string serialized)
    {
        if(serialized != "")
            m_Serialized = serialized;

        if(m_Serialized == null)
            return;

        string[] notes = m_Serialized.Split(' ');
        m_Pattern = new PatternNote[notes.Length];
        if(notes != null)
        {
            for(int i = 0; i < notes.Length; i ++)
            {
                if(notes[i] != "")
                {
                    m_Pattern[i] = new PatternNote(notes[i]);
                }
            }
        }        
    }

    public override string ToString()
    {
        string s = "";
        if(m_Pattern != null)
        {            
            for(int i = 0; i < m_Pattern.Length; i ++)
            {
                if(m_Pattern[i] != null)
                {
                    s += m_Pattern[i].ToString();
                }
            }
        }

        return s;
    }
}

[System.Serializable]
public class ItemData
{
    public bool m_Enabled = false;

    public string m_UniqueID = System.Guid.Empty.ToString();

	public string m_Name = "DefaultItemName";
    public Sprite m_ItemIcon;
    public bool m_IsStackable = true;
    public ItemType m_TypeFlags = 0x0;
    //public AnimationClip m_CraftPattern = null;
    public CraftPattern m_CraftPattern = null;

    public string m_InBuildingID = null;
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
    Building = 0x8,
}

public class ItemDatabase : AssetSingleton<ItemDatabase>
{		
	[SerializeField]
	private List<ItemData>					m_items			= new List<ItemData>();

	private	Dictionary<string, ItemData>	m_itemDatabase	= new Dictionary<string, ItemData>();

    private Dictionary<string, BuildingData> m_allBuildings = new Dictionary<string,BuildingData>();

    public List<Buttons> m_ButtonsPerIndex = new List<Buttons>();

	private	void OnEnable()
	{
		Refresh();
        InitializeBuildingData();
	}

    public void Refresh()
    {
        m_itemDatabase.Clear();
        foreach (ItemData item in m_items)
        {
            m_itemDatabase.Add(item.m_UniqueID, item);
            item.m_CraftPattern.Unserialize(item.m_CraftPattern.m_Serialized);
        }
    }

    void InitializeBuildingData()
    {
        m_allBuildings = new Dictionary<string, BuildingData>();
        
        List<ItemData> allItems = ItemDatabase.GetAllItems();

        ItemData data = null;
        BuildingData buildingData = null;

        for(int i = 0; i < allItems.Count; i++)
        {            
            data = allItems[i];

            if(!m_allBuildings.TryGetValue(allItems[i].m_InBuildingID, out buildingData))
            {
                buildingData = new BuildingData(ItemDatabase.GetItemByUniqueID(allItems[i].m_InBuildingID));
                m_allBuildings.Add(allItems[i].m_InBuildingID, buildingData);
            }
            
            buildingData.AddRecipe(data);            
            
            data = null;
            buildingData = null;
        }

        foreach(KeyValuePair<string, BuildingData> buildings in m_allBuildings)
        {
            buildings.Value.AssignButtons(m_ButtonsPerIndex);
        }
    }

    public static BuildingData GetBuildingByUniqueID(string uniqueID)
    {
        ItemDatabase inst = Instance;

        if(inst != null)
        {
            if(Instance.m_allBuildings.ContainsKey(uniqueID))
            {
                return Instance.m_allBuildings[uniqueID];
            }
        }

        return null;
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
