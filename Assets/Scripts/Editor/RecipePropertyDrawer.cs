using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(RecipeData))]
public class RecipePropertyDrawer : PropertyDrawer 
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUILayout.LabelField("Hallo");
        EditorGUI.EndProperty();
    }
}
