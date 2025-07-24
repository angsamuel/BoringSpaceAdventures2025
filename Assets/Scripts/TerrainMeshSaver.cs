#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TerrainMeshSaver : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public bool saveMesh = false;
    [ContextMenu("Save Mesh As Asset")]


    void Awake(){
        terrainGenerator = GetComponent<TerrainGenerator>();
    }

    void Update(){
        if(saveMesh){
            SaveMesh();
            saveMesh = false;
        }
    }


    public void SaveMesh()
    {
#if UNITY_EDITOR
        Mesh mesh = terrainGenerator.meshFilter.sharedMesh;

        if (mesh == null)
        {
            Debug.LogWarning("No mesh found to save.");
            return;
        }

        string path = "Assets/SavedMesh.asset";
        Mesh meshCopy = Instantiate(mesh);  // Make sure we don't overwrite internal mesh
        AssetDatabase.CreateAsset(meshCopy, path);
        AssetDatabase.SaveAssets();
        Debug.Log("Mesh saved to " + path);

        // Reassign saved mesh to MeshFilter (to ensure it is the saved asset, not runtime copy)
        terrainGenerator.meshFilter.sharedMesh = meshCopy;
#else
        Debug.LogWarning("Mesh saving only works in the Unity Editor.");
#endif
    }
}
