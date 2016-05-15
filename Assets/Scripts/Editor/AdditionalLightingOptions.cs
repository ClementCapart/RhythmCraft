using UnityEngine;
using UnityEditor;
using System.Collections;

public class AdditionalLightingOptions : EditorWindow 
{
    [MenuItem ("Tools/AdditionalLightingOptions")]
	static void Init () 
    {
		// Get existing open window or if none, make a new one:
		AdditionalLightingOptions window = (AdditionalLightingOptions)EditorWindow.GetWindow (typeof (AdditionalLightingOptions));
		window.Show();
	}

    void OnGUI()
    {
        if(GUILayout.Button("Bake Selected"))
        {
            Lightmapping.BakeSelectedAsync();
        }

        EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(true), Lightmapping.buildProgress, "Lightmap Baking Progress");

        if(GUILayout.Button("Clear Scene Lighting Data"))
        {
            Lightmapping.Clear();
        }

        if(GUILayout.Button("Cancel"))
        {
            Lightmapping.Cancel();
        }
    }
}
