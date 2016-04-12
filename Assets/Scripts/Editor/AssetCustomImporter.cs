using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class AssetCustomImporter : AssetPostprocessor 
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;

        if(modelImporter)
        {
            modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
            modelImporter.useFileUnits = true;
        }
    }

    void OnPostprocessModel(GameObject result)
    {
        /*ModelImporter modelImporter = assetImporter as ModelImporter;
        if(Path.GetExtension(modelImporter.assetPath) == ".max")
        {
            RotateObject(result.transform);
        }

        result.transform.rotation = Quaternion.identity;*/
    }

    private void RotateObject(Transform transform, bool meshOnly = false)
    {
        if(!meshOnly)
        {
            Vector3 objectRotation = transform.eulerAngles;
            objectRotation.x += 90.0f;
            transform.eulerAngles = objectRotation;

            //transform.Rotate(Vector3.right, 90.0f);
        }

        Vector3 newPos = new Vector3(transform.position.x, transform.position.z, transform.position.y);
        transform.position = newPos;        

        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        if(meshFilter)
        {
            RotateMesh(meshFilter.sharedMesh);
        }

        foreach(Transform child in transform)
        {
            child.transform.position = Quaternion.AngleAxis(90.0f, transform.right) * transform.position;
            RotateObject(child, true);
        }
    }

    private void RotateMesh(Mesh mesh)
    {
        int index = 0;

        Vector3[] vertices= mesh.vertices;
        for(index = 0; index < vertices.Length; index++)
        {
            vertices[index] = Quaternion.AngleAxis(-90.0f, Vector3.right) * vertices[index];
        }

        mesh.vertices = vertices;        

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
