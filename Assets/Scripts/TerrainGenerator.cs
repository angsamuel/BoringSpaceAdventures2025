using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Generation Settings")]
    public bool needsGeneration = true;
    public int seed;
    public float maxHeight = 1;


    public float noiseScale1 = 1;
    public float noiseIntensity1 = 1;

    public float noiseScale2 = 1f;
    public float noiseIntensity2 = 1f;

    [Header("Texture")]

    public Texture2D texture;
    public float textureScale = 1f;

    public float textureIntensity = 1f;





    [Header("Objects")]
    public MeshFilter meshFilter;
    Mesh mesh;
    void Start()
    {
        mesh = Instantiate(meshFilter.mesh);
        meshFilter.mesh = mesh;
    }

    public void GenerateTerrain(){
        Random.InitState(seed);

        Vector3[] vertices = mesh.vertices;
        //vertices[0].y = 10;

        float offsetX1 = Random.Range(0,10000f);
        float offsetZ1 = Random.Range(0, 10000f);
        float offsetX2 = Random.Range(0, 10000f);
        float offsetZ2 = Random.Range(0, 10000f);

        int offsetTextureX = Random.Range(0, texture.width);
        int offsetTextureZ = Random.Range(0, texture.height);


        for (int i = 0; i<vertices.Length; i++){
            float x = (vertices[i].x + 1) / 2;
            float z = (vertices[i].z + 1) / 2;

            float tx = x * textureScale;
            float tz = z * textureScale;


            vertices[i].y = texture.GetPixel((int)(tx * textureScale * texture.width), (int)(tz * textureScale * texture.height)).grayscale * textureIntensity;


            float noiseValue1 = Mathf.PerlinNoise(x * noiseScale1 + offsetX1, z * noiseScale1 + offsetZ1);
            float noiseValue2 = Mathf.PerlinNoise(x * noiseScale2 + offsetX2, z * noiseScale2 + offsetZ2);
            vertices[i].y += ((noiseValue1*noiseIntensity1) + (noiseValue2*noiseIntensity2)) * maxHeight;
        }


        //we are done with generation
        mesh.vertices = vertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.transform.localPosition = new Vector3(
            0,
            -(maxHeight * (noiseIntensity1 + noiseIntensity2) + textureIntensity)/2 * meshFilter.transform.localScale.y,
            0
            );
    }

    // Update is called once per frame
    void Update()
    {
        if(needsGeneration){
            GenerateTerrain();
            needsGeneration = false;
        }
    }
}
