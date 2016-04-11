using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UIPrefab))]
public class UIPrefabInspector : Editor
{
    const int   MAX_DEPTH_DISPLAYED = 10;
    const float BUTTON_WIDTH        = 200.0f;
    const float DEPTH_BUTTON_OFFSET = 25.0f;

    bool m_foldOutPrefabInspector = true;

    Transform m_transform = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        m_transform = (target as UIPrefab).transform;
        
        if(PrefabUtility.GetPrefabType(m_transform) == PrefabType.Prefab || PrefabUtility.GetPrefabType(m_transform) == PrefabType.ModelPrefab || (!Application.isPlaying && (PrefabUtility.GetPrefabType(m_transform) == PrefabType.PrefabInstance || PrefabUtility.GetPrefabType(m_transform) == PrefabType.ModelPrefabInstance)))
        {
            m_foldOutPrefabInspector = EditorGUILayout.Foldout(m_foldOutPrefabInspector, "Prefab Inspector");

            if(m_foldOutPrefabInspector)
            {
                GameObject prefabRoot = PrefabUtility.FindPrefabRoot(m_transform.gameObject);
                if(prefabRoot != m_transform.gameObject)
                {
                    EditorGUILayout.HelpBox("Helpers", MessageType.None);

                    if(GUILayout.Button("Root: " + prefabRoot.name))
                    {            
                        Selection.activeGameObject = prefabRoot;
                    }  

                    if(prefabRoot.transform != m_transform.parent && m_transform != null)
                    {
                        if(GUILayout.Button("Parent: " + m_transform.parent.name))
                        {            
                            Selection.activeGameObject = m_transform.parent.gameObject;
                        }  
                    }

                    EditorGUILayout.Space();
                }

                EditorGUILayout.HelpBox("Prefab Hierachy", MessageType.None);

                if(m_transform.childCount == 0)
                {                    
                    EditorGUILayout.LabelField("No child");
                }
                else
                { 
                    GeneratePrefabObjectsButtons(m_transform, 0);                
                }
                
            }
        }
        
    }	

    void GeneratePrefabObjectsButtons(Transform currentTransform, int currentDepth)
    {
        if(currentDepth > MAX_DEPTH_DISPLAYED)
        {
            if (currentTransform.childCount > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(currentDepth * DEPTH_BUTTON_OFFSET);
                EditorGUILayout.HelpBox("Deeper existing hierarchy not displayed", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
            }
            return;
        }
            
        foreach(Transform t in currentTransform)
        {            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(currentDepth * DEPTH_BUTTON_OFFSET + DEPTH_BUTTON_OFFSET);            

            if(GUILayout.Button(t.name, GUILayout.Width(BUTTON_WIDTH)))
            {            
                Selection.activeGameObject = t.gameObject;
            }                        

            EditorGUILayout.EndHorizontal();
            GeneratePrefabObjectsButtons(t, currentDepth + 1);
        }
    }

}
