#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class TerrainTestGen : MonoBehaviour
{

    public Texture2D heightMap;
    public float maxHeight = 1f;
    public float perlinNoiseScale1 = 1;
    public float perlinNoiseScale2 = 1;

    public float perlinNoiseWeight1 = 1;
    public float perlinNoiseWeight2 = 1;

    public float textureScale = 1f;

    public float heightMapImpact = 1f;

    bool copied = false;
    public void ApplyRandomHeights()
    {
        Mesh mesh = null;
        MeshFilter mf = GetComponent<MeshFilter>();
        if (!copied){
            mesh = Instantiate(mf.sharedMesh);
            mf.mesh = mesh;
            copied = true;
        }

        mesh = mf.mesh;



        float offsetX1 = Random.Range(-1000, 1000);
        float offsetZ1 = Random.Range(-1000, -1000);

        float offsetX2 = Random.Range(-1000, 1000);
        float offsetZ2 = Random.Range(-1000, -1000);



        Vector3[] vertices = mesh.vertices;

        float maxX = 0;

        for (int i = 0; i < vertices.Length; i++)
        {

            Vector3 v = vertices[i];


            float noiseValue1 = Mathf.PerlinNoise(v.x * perlinNoiseScale1 + offsetX1, v.z * perlinNoiseScale1 + offsetZ1);


            float noiseValue2 = Mathf.PerlinNoise(v.x * perlinNoiseScale2 + offsetX2, v.z * perlinNoiseScale2 + offsetZ2);



            vertices[i].y = (noiseValue1 * perlinNoiseWeight1 + noiseValue2 * perlinNoiseWeight2)  * maxHeight;




            int pixelX = (int)(heightMap.width *  ((v.x + 1) / 2) * textureScale);
            int pixelZ = (int)(heightMap.height * ((v.z + 1) / 2) * textureScale);

            float height = heightMap.GetPixel(pixelX, pixelZ).grayscale;
            vertices[i].y += height * heightMapImpact;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        transform.position = new Vector3(0, -((perlinNoiseWeight1 + perlinNoiseWeight2) / 2) - (heightMapImpact/2));


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TerrainTestGen))]
    public class TerrainTestGenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TerrainTestGen script = (TerrainTestGen)target;

            if (GUILayout.Button("Apply Random Heights"))
            {
                script.ApplyRandomHeights();
            }
        }
    }
#endif
}
