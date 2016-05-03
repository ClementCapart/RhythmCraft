using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeInfoPopup : ContextualPopup 
{    
    private ItemData m_item = null;

    public Text m_ItemNameText = null;    
    public Text m_TypesText = null; //< - Placeholders - To replace by icons pretty please : D
    public Text m_RecipeElementsText = null;

    public void SetData(ItemData itemData)
    {
        m_item = itemData;

        if(m_ItemNameText) 
            m_ItemNameText.text = GetItemName();

        if(m_TypesText)
            m_TypesText.text = GetTypesString();

        if(m_RecipeElementsText)
            m_RecipeElementsText.text = GetRecipeString();
    }

    public string GetItemName()
    {
        if(m_item.m_AlreadyCrafted)
        { 
            return m_item.m_Name;
        }
        else
        {
            return "??????";
        }
        
    }

    public string GetTypesString() //< - Placeholder
    {
        string s = "";

        if((m_item.m_TypeFlags & ItemType.Usable) != 0)
        {
            s += "U";
        }

        if((m_item.m_TypeFlags & ItemType.Entertainment) != 0)
        {
            if(s !=  "")
            {
                s += " ";
            }

            s += "E";
        }

        if((m_item.m_TypeFlags & ItemType.Ingredient) != 0)
        {
            if(s !=  "")
            {
                s += " ";
            }

            s += "I";
        }

        if((m_item.m_TypeFlags & ItemType.Building) != 0)
        {
            if(s !=  "")
            {
                s += " ";
            }

            s += "B";
        }

        return s;
    }

    public string GetRecipeString()
    {
        string recipe = "";

        foreach(RecipeElement recipeItem in m_item.m_Recipe.m_ItemsNeeded)
        {
            if(recipe == "")
            {
                recipe = "Recipe:";
            }
            
            ItemData item = ItemDatabase.GetItemByUniqueID(recipeItem.m_itemID);
            if(item != null)
            {
                recipe += "\n";
                if(item.m_AlreadyCrafted)
                {
                    recipe += item.m_Name;
                }
                else
                {
                    recipe += "??????";
                }   
             
                recipe += " x" + recipeItem.m_itemCount;
            }            
        }

        return recipe;
    }


}
