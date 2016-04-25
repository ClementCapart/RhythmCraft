using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeControllerHUDSection : HUDSection 
{
    List<ButtonRecipeUI> m_ButtonsUI = new List<ButtonRecipeUI>();
    public GameObject m_CraftSetUIPrefab;

    List<CraftSetControllerUI> m_CurrentBuildDataSetsUI = new List<CraftSetControllerUI>(); 

    public Vector2 m_CraftSetUIOffset = Vector2.zero;

    void Awake()
    {
        m_ButtonsUI = new List<ButtonRecipeUI>(GetComponentsInChildren<ButtonRecipeUI>(true));

        BuildingController.s_onBuildingSelected += OnBuildingSelected;
        RecipeController.s_OnCraftSetSelected += OnCraftSetSelected;
    }

    void OnDestroy()
    {
        BuildingController.s_onBuildingSelected -= OnBuildingSelected;
        RecipeController.s_OnCraftSetSelected -= OnCraftSetSelected;
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

    void GenerateSetsUI(BuildingData buildingData)
    {
        ClearSetsUI();

        if(buildingData != null)
        {
            List<BuildingData.CraftSet> craftSets = buildingData.m_CraftSets;

            for(int i = 0; i < craftSets.Count; i++)
            {
                GameObject obj = Instantiate<GameObject>(m_CraftSetUIPrefab);
                CraftSetControllerUI uiScript = obj.GetComponent<CraftSetControllerUI>();
                obj.transform.SetParent(transform, false);
                uiScript.m_RectTransform.anchoredPosition = new Vector2(i * m_CraftSetUIOffset.x, i * m_CraftSetUIOffset.y);
                uiScript.InitializeIcons(craftSets[i]);
                m_CurrentBuildDataSetsUI.Add(uiScript);
            }
        }
    }

    void ClearSetsUI()
    {
        for(int i = 0; i < m_CurrentBuildDataSetsUI.Count; i++)
        {
            Destroy(m_CurrentBuildDataSetsUI[i].gameObject);
        }

        m_CurrentBuildDataSetsUI.Clear();
    }

    void OnBuildingSelected(BuildingData buildingData)
    {
        GenerateSetsUI(buildingData);
    }

    void OnCraftSetSelected(BuildingData.CraftSet craftSet)
    {
        bool found = false;
        int currentSiblingIndexToSet = transform.childCount - 1;
        int offsetMultiplier = 0;
        int indexToStopTo = -1;

        for (int i = 0; i < m_CurrentBuildDataSetsUI.Count; i++)
        {
            if(found)
            {
                SetCraftSetUI(m_CurrentBuildDataSetsUI[i], currentSiblingIndexToSet, offsetMultiplier, false);

                currentSiblingIndexToSet--;                
                offsetMultiplier++;
            }
            else if(m_CurrentBuildDataSetsUI[i].m_CraftSet == craftSet)
            {
                found = true;
                indexToStopTo = i;

                SetCraftSetUI(m_CurrentBuildDataSetsUI[i], currentSiblingIndexToSet, offsetMultiplier, true);
                
                currentSiblingIndexToSet--;
                offsetMultiplier++;
            }
        }

        for(int i = 0; i < indexToStopTo; i++)
        {
            SetCraftSetUI(m_CurrentBuildDataSetsUI[i], currentSiblingIndexToSet, offsetMultiplier, false);

            currentSiblingIndexToSet--;                
            offsetMultiplier++;
        }
    }
	
    void SetCraftSetUI(CraftSetControllerUI setUI, int siblingIndex, int offsetMultiplier, bool fullDisplay)
    {
        setUI.m_RectTransform.SetSiblingIndex(siblingIndex);
        //setUI.m_RectTransform.anchoredPosition = m_CraftSetUIOffset * offsetMultiplier;
        setUI.MoveTo(m_CraftSetUIOffset * offsetMultiplier, 0.3f);

        if(fullDisplay)
        {
            setUI.FullDisplay();
        }
        else
        {
            setUI.PartialDisplay();
        }
    }
}
