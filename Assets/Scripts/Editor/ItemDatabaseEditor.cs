using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    private SerializedProperty m_itemListProperty = null;

    private ReorderableList m_itemList = null;

    private int m_currentlySelected = 0;

    public void OnEnable()
    {
        m_itemListProperty = this.serializedObject.FindProperty("m_items");

        if (this.m_itemList == null)
        {
            this.m_itemList = new ReorderableList(this.serializedObject, this.m_itemListProperty, true, false, true, true);
            this.m_itemList.onAddCallback = this.AddToList;
            this.m_itemList.onRemoveCallback = this.RemoveFromList;
            this.m_itemList.drawElementCallback = this.DrawElement;
            this.m_itemList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
            this.m_itemList.headerHeight = 3f;
        }
    }

    public override void OnInspectorGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyUp && e.keyCode == (KeyCode.F5))
        {
            (target as ItemDatabase).Refresh();
        }

        this.serializedObject.Update();

        ItemData item = ItemDatabase.GetItemByIndex(m_currentlySelected);

        if (item != null)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    GUI.enabled = false;
                    EditorGUILayout.TextField("Unique ID", item.m_UniqueID.ToString());
                    GUI.enabled = true;

                    item.m_Name = EditorGUILayout.TextField("Name", item.m_Name);                    
                    item.m_ItemIcon = EditorGUILayout.ObjectField("Icon", item.m_ItemIcon, typeof(Texture), false) as Texture;
                    item.m_IsStackable = EditorGUILayout.Toggle("Is Stackable", item.m_IsStackable);
                    item.m_TypeFlags = (ItemType)EditorGUILayout.EnumMaskField("Type", item.m_TypeFlags);
                    item.m_CraftPattern = EditorGUILayout.ObjectField("Craft Pattern", item.m_CraftPattern, typeof(AnimationClip), false) as AnimationClip;                    

                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0f);            
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        this.m_itemList.DoLayoutList();
        this.serializedObject.ApplyModifiedProperties();
    }

    private void AddToList(ReorderableList list)
    {
        ItemDatabase.CreateNewItem();

        this.serializedObject.ApplyModifiedProperties();
        this.serializedObject.Update();
        list.index = list.serializedProperty.arraySize - 1;
    }

    private void RemoveFromList(ReorderableList list)
    {       
        ItemDatabase.RemoveItem(ItemDatabase.GetItemByIndex(m_currentlySelected));

        this.serializedObject.ApplyModifiedProperties();
        this.serializedObject.Update();
        list.index = list.serializedProperty.arraySize - 1;
    }

    private void DrawElement(Rect rect, int index, bool selected, bool focused)
    {
        ItemData item = ItemDatabase.GetItemByIndex(index);

        if (item != null)
        {
            if (!string.IsNullOrEmpty(item.m_Name))
            {
                GUI.Label(rect, item.m_Name);
            }
            else
            {
                GUI.Label(rect, item.m_UniqueID.ToString());
            }

            if (selected)
            {
                if (m_currentlySelected != index)
                {
                    m_currentlySelected = index;
                    this.Repaint();
                }
            }
        }
    }
}
