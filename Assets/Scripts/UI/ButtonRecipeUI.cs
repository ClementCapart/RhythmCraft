using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonRecipeUI : MonoBehaviour 
{
    public Buttons m_Button = Buttons.None;
    public RecipeIconUI m_RecipeObject = null;
    public Image m_ButtonImage = null;

    private ItemData m_item = null;

    void Start()
    {
        if(m_item == null)
        { 
            UpdateRecipe(null);
        }                
    }

	public void UpdateRecipe(ItemData itemData)
    {
        m_item = itemData;
        
        if(m_RecipeObject != null)
        {
            m_RecipeObject.UpdateRecipe(itemData);
            if(m_item != null)
                m_ButtonImage.color = Color.black;
            else
                m_ButtonImage.color = Color.red;
        }
    }
}
