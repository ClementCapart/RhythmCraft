using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetCustomImporter : AssetPostprocessor 
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;

        if(modelImporter)
        {
            modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
        }
    }
}
