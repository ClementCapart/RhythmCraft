using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(RecipeElement))]
public class RecipeElementPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {              
        List<ItemData> items = ItemDatabase.GetAllItems();    
        string[] itemNames = new string[items.Count];
        for(int i = 0; i < items.Count; i++)
        {
            itemNames[i] = items[i].m_Name;
        }

        ItemData item = ItemDatabase.GetItemByIndex(EditorGUILayout.Popup("Item", ItemDatabase.GetIndexByUniqueID(property.FindPropertyRelative("m_itemID").stringValue), itemNames));
        property.FindPropertyRelative("m_itemCount").intValue = EditorGUILayout.IntField("Count", property.FindPropertyRelative("m_itemCount").intValue);

        if(item != null)
        { 
            property.FindPropertyRelative("m_itemID").stringValue = item.m_UniqueID;
        }             
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) - 18.0f;
    }
}
