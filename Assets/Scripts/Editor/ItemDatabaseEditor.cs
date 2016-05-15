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

    [MenuItem("Databases/ItemDatabase")]
    public static void OpenDatabase()
    {
        Selection.activeObject = ItemDatabase.Instance;
    }

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

                    item.m_Enabled = EditorGUILayout.Toggle("Enabled", item.m_Enabled);
                    item.m_Name = EditorGUILayout.TextField("Name", item.m_Name);                    
                    item.m_ItemIcon = EditorGUILayout.ObjectField("Icon", item.m_ItemIcon, typeof(Sprite), false) as Sprite;
                    item.m_IsStackable = EditorGUILayout.Toggle("Is Stackable", item.m_IsStackable);
                    item.m_TypeFlags = (ItemType)EditorGUILayout.EnumMaskField("Type", item.m_TypeFlags);

                    if((item.m_TypeFlags & ItemType.Base) != 0)
                    {
                        item.m_FailedBaseIcon = EditorGUILayout.ObjectField("Failed Base Icon", item.m_FailedBaseIcon, typeof(Sprite), false) as Sprite;
                    }

                    //item.m_CraftPattern = EditorGUILayout.ObjectField("Craft Pattern", item.m_CraftPattern, typeof(AnimationClip), false) as AnimationClip;                    
                    if(item.m_CraftPattern == null)
                    { 
                        item.m_CraftPattern = new CraftPattern(EditorGUILayout.TextField("Craft Pattern", item.m_CraftPattern.m_Serialized));
                    }
                    else
                    {
                        item.m_CraftPattern.Unserialize(EditorGUILayout.TextField("Craft Pattern", item.m_CraftPattern.m_Serialized));
                    }
                    
                    List<ItemData> items = ItemDatabase.GetAllItems();    
            
                    string[] itemNames = new string[items.Count + 1];
                    itemNames[0] = "None";
                    for(int i = 0; i < items.Count; i++)
                    {
                        itemNames[i + 1] = items[i].m_Name;
                    }

                    ItemData building = ItemDatabase.GetItemByIndex(EditorGUILayout.Popup("Building", item.m_InBuildingID != "" ? ItemDatabase.GetIndexByUniqueID(item.m_InBuildingID) + 1 : 0, itemNames) - 1);
                    item.m_InBuildingID = building != null ? building.m_UniqueID : "";

                    
                    SerializedProperty recipeProp = m_itemListProperty.GetArrayElementAtIndex(m_currentlySelected).FindPropertyRelative("m_Recipe");
                    EditorGUILayout.PropertyField(recipeProp, true);
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

        SerializedProperty buttons = this.serializedObject.FindProperty("m_ButtonsPerIndex");
        EditorGUILayout.PropertyField(buttons, true);

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
        list.DoLayoutList();
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
