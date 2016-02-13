using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(RecipeElementPropertyDrawer))]
public class RecipeElementPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUILayout.LabelField("Hallo");
        EditorGUI.EndProperty();
    }
}
