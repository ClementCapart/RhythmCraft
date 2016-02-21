using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(RecipeData))]
public class RecipePropertyDrawer : PropertyDrawer 
{
    bool m_showRecipeList = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        m_showRecipeList = EditorGUILayout.Foldout(m_showRecipeList, "Recipe");       

        if (m_showRecipeList)
        {
            SerializedProperty list = property.FindPropertyRelative("m_ItemsNeeded");
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            EditorGUI.indentLevel++;
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }        
    }
}
