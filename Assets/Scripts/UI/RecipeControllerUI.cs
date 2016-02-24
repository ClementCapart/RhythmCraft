using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeControllerUI : MonoBehaviour 
{
    List<ButtonRecipeUI> m_ButtonsUI = new List<ButtonRecipeUI>();

    void Start()
    {
        RecipeController.s_usableRecipesUpdated += UpdateUI;

        m_ButtonsUI = new List<ButtonRecipeUI>(GetComponentsInChildren<ButtonRecipeUI>(true));
    }

    void OnDestroy()
    {
        RecipeController.s_usableRecipesUpdated -= UpdateUI;
    }

    void UpdateUI(Dictionary<Buttons, ItemData> usableCrafts)
    {
        foreach(KeyValuePair<Buttons, ItemData> kvp in usableCrafts)
        {
            for(int i = 0; i < m_ButtonsUI.Count; i++)
            {
                if(m_ButtonsUI[i].m_Button == kvp.Key)
                {
                    m_ButtonsUI[i].UpdateRecipe(kvp.Value);
                    break;
                }
            }
        }
    }
	
}
