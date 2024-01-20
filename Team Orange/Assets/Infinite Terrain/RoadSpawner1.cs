using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner1 : MonoBehaviour
{
    private const int REVERSE = -1;
    private const int TILE_MIN_X = 0;
    private const int TILE_MAX_X = 1000;
    private const int ON_TILE = 25;

    [SerializeField] private List<GameObject> playableTiles;
    public static List<GameObject> tilesList;
    [SerializeField] private List<GameObject> renderedTiles;

    [SerializeField] private List<GameObject> countryBuildings;
    [SerializeField] private List<GameObject> desertTrees;

    [SerializeField] private float mountainZPos;

    [SerializeField] private float buildingZPos;
    [SerializeField] private int buildingSizeX;

    [SerializeField] private float treeZPos;
    [SerializeField] private float treeSpacingZ;
    [SerializeField] private float treeSpacingX;

    [SerializeField] private int tileSizeZ;
    [SerializeField] private int numTiles;

    [SerializeField] private float leftRoadOffsetX;
    [SerializeField] private float rightRoadOffsetX;

    [SerializeField] private int noiseXSize;
    [SerializeField] private int startNoiseX;
    [SerializeField] private int noiseZSize;
    [SerializeField] private int startNoiseZ;
    [SerializeField] private int switchSideX;
    [SerializeField] private int countryOffsetX;
    [SerializeField] private int desertOffsetX;

    [SerializeField] private int noiseSpacing;
    [SerializeField] private int LHSstopNoise;
    [SerializeField] private int RHSstopNoise;

    private float scale;
    private float persistance;
    private float lacunarity;

    [SerializeField] private int octaves;

    [SerializeField] private int offset;

    private static GameObject selectedTile;

    [SerializeField] private Material desert;

    // Related to 'Water' tagged terrain
    [SerializeField] private float sandRoadDist;
    [SerializeField] private float treeOnSandX;
    [SerializeField] private float treeMaxForwardX;
    [SerializeField] private float treeMaxBackwardX;

    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        scale = Random.Range(20, 40);
        persistance = Random.Range(2.5f, 4.5f);
        lacunarity = Random.Range(1f, 1.2f);

        tilesList = new List<GameObject>(playableTiles);
        int index = Random.Range(0, playableTiles.Count);
        selectedTile = playableTiles[index];

        for (int i = 0; i < numTiles; i++)
        {
            GameObject tile = Instantiate(selectedTile, this.transform);
            tile.tag = selectedTile.tag;
            tile.transform.position = new Vector3(-500.0f, 0.0f, tileSizeZ * i);
            tile.SetActive(true);
            DecorateTile(tile, -500, tileSizeZ * i);
            renderedTiles.Add(tile);
        }
    }

    private void addNoise(GameObject tile, int tilePosX, int tilePosZ)
    {
        GameObject noise = new GameObject("Noise");
        noise.transform.SetParent(tile.transform);

        noise.AddComponent(typeof(MeshFilter));
        noise.AddComponent(typeof(MeshRenderer));
        noise.AddComponent(typeof(MeshCollider));

        Mesh mesh = new Mesh();

        createShape(tilePosX + startNoiseX, tilePosZ + startNoiseZ);
        updateMesh(mesh);

        noise.GetComponent<MeshFilter>().mesh = mesh;
        noise.GetComponent<MeshCollider>().sharedMesh = mesh;

        if (tile.CompareTag("Country"))
        {
            noise.GetComponent<MeshRenderer>().material = desert;
            noise.transform.position = new Vector3(tilePosX + startNoiseX - countryOffsetX, -0.1f, tilePosZ + startNoiseZ);

            GameObject nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX - countryOffsetX - noiseSpacing, -0.1f, tilePosZ + startNoiseZ);

            nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX + switchSideX + countryOffsetX, -0.1f, tilePosZ + startNoiseZ);

            nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX + switchSideX + countryOffsetX + noiseSpacing, -0.1f, tilePosZ + startNoiseZ);
        }

        else if (tile.CompareTag("Desert"))
        {
            noise.GetComponent<MeshRenderer>().material = desert;
            noise.transform.position = new Vector3(tilePosX + startNoiseX - desertOffsetX, -0.1f, tilePosZ + startNoiseZ);

            GameObject nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX - desertOffsetX - noiseSpacing, -0.1f, tilePosZ + startNoiseZ);

            nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX + switchSideX + desertOffsetX, -0.1f, tilePosZ + startNoiseZ);

            nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX + switchSideX + desertOffsetX + noiseSpacing, -0.1f, tilePosZ + startNoiseZ);
        }

        else if (tile.CompareTag("Water"))
        {
            noise.GetComponent<MeshRenderer>().material = desert;
            noise.transform.position = new Vector3(tilePosX + startNoiseX - desertOffsetX - sandRoadDist, -0.1f, tilePosZ + startNoiseZ);

            GameObject nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX - desertOffsetX - noiseSpacing - sandRoadDist, -0.1f, tilePosZ + startNoiseZ);

            nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX + switchSideX + desertOffsetX + sandRoadDist, -0.1f, tilePosZ + startNoiseZ);

            nextNoise = Instantiate(noise, tile.transform);
            nextNoise.transform.position = new Vector3(tilePosX + startNoiseX + switchSideX + desertOffsetX + noiseSpacing + sandRoadDist, -0.1f, tilePosZ + startNoiseZ);
        }
    }

    private void createShape(int noiseStartX, int noiseStartZ)
    {
        vertices = new Vector3[(noiseXSize + 1) * (noiseZSize + 1)];

        for (int i = 0, z = noiseStartZ; z < noiseStartZ + noiseZSize + 1; z++)
        {
            for (int x = noiseStartX; x < noiseStartX + noiseXSize + 1; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                int k = 0;
                int l = 0;

                for (int j = 0; j < octaves; j++)
                {
                    float sampleX = (x / scale) * frequency;
                    float sampleZ = (z / scale) * frequency;
                    float perlinValue = -(Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1);
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if ((x > noiseStartX + noiseXSize + 1 - offset) && (noiseHeight > 0))
                {
                    float reduce = noiseHeight / offset;
                    noiseHeight = reduce * k;
                    k += 1;

                }

                if ((x >= noiseStartX && x <= noiseStartX + offset) && (noiseHeight > 0))
                {
                    float reduce = noiseHeight / offset;
                    noiseHeight = reduce * -l;
                    l += 1;

                }

                vertices[i] += new Vector3(x - noiseStartX, noiseHeight, z - noiseStartZ);
                i++;
            }
        }

        triangles = new int[noiseXSize * noiseZSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < noiseZSize; z++)
        {
            for (int x = 0; x < noiseXSize; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + noiseXSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + noiseXSize + 1;
                triangles[tris + 5] = vert + noiseXSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void updateMesh(Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public static void RandomiseSelectedTile()
    {
        int index = Random.Range(0, tilesList.Count);
        selectedTile = tilesList[index];
    }
    public void MoveTile()
    {
        GameObject deleteTile = renderedTiles[0];
        renderedTiles.Remove(deleteTile);
        Destroy(deleteTile);

        GameObject newTile = Instantiate(selectedTile, this.transform);
        float newTileZ = renderedTiles[renderedTiles.Count - 1].transform.position.z + tileSizeZ;
        newTile.transform.position = new Vector3(-500, 0.0f, newTileZ);
        DecorateTile(newTile, -500, (int)newTileZ);
        newTile.SetActive(true);

        renderedTiles.Add(newTile);
    }

    private void DecorateTile(GameObject tile, int tilePosX, int tilePosZ)
    {
        if (tile.CompareTag("Country"))
        {
            MakeCountry(tile, tilePosX, tilePosZ);
        }

        else if (tile.CompareTag("Desert"))
        {
            MakeDesert(tile, tilePosX, tilePosZ);
        }

        else if (tile.CompareTag("Water"))
        {
            MakeWater(tile, tilePosX, tilePosZ);
        }
    }

    private void MakeWater(GameObject tile, int tilePosX, int tilePosZ)
    {
        int index;
        float treePosZ;

        addNoise(tile, tilePosX, tilePosZ);

        float treePosX = -treeOnSandX + Random.Range(-treeMaxForwardX, treeMaxBackwardX);
        // LHS trees
        index = Random.Range(0, desertTrees.Count);
        GameObject tree = Instantiate(desertTrees[index], tile.transform);
        tree.transform.localScale *= Random.Range(1f, 2f);
        Quaternion rotation = tree.transform.rotation;
        tree.transform.rotation = new Quaternion(rotation.x, Random.Range(0.0f, 360.0f), rotation.z, rotation.w);
        treePosZ = tile.transform.position.z + treeZPos + Random.Range(0, treeSpacingZ);
        tree.transform.position = new Vector3(treePosX, getPerlinValue(treePosX - 500.0f, treePosZ), treePosZ);

        treePosX = treeOnSandX + Random.Range(treeMaxForwardX, -treeMaxBackwardX);
        // LHS trees
        index = Random.Range(0, desertTrees.Count);
        tree = Instantiate(desertTrees[index], tile.transform);
        tree.transform.localScale *= Random.Range(1f, 2f);
        rotation = tree.transform.rotation;
        tree.transform.rotation = new Quaternion(rotation.x, Random.Range(0.0f, 360.0f), rotation.z, rotation.w);
        treePosZ = tile.transform.position.z + treeZPos + Random.Range(0, treeSpacingZ);
        tree.transform.position = new Vector3(treePosX, getPerlinValue(treePosX - 500.0f, treePosZ), treePosZ);
    }

    private void MakeDesert(GameObject tile, int tilePosX, int tilePosZ)
    {
        int index;
        float treePosZ;
        addNoise(tile, tilePosX, tilePosZ);

        Terrain terrain = tile.GetComponent<Terrain>();

        // LHS trees
        float treePosX = leftRoadOffsetX;
        while (treePosX > TILE_MIN_X)
        {
            if (chance())
            {
                index = Random.Range(0, desertTrees.Count);
                GameObject tree = Instantiate(desertTrees[index], tile.transform);

                tree.transform.localScale *= Random.Range(0.25f, 2f);
                Quaternion rotation = tree.transform.rotation;
                tree.transform.rotation = new Quaternion(rotation.x, Random.Range(0.0f, 360.0f), rotation.z, rotation.w);

                treePosZ = tile.transform.position.z + treeZPos + Random.Range(0, treeSpacingZ);
                tree.transform.position = new Vector3(treePosX - 500.0f, getPerlinValue(treePosX - 500.0f, treePosZ), treePosZ);
            }

            treePosX -= treeSpacingX;
        }

        // RHS trees
        treePosX = rightRoadOffsetX;
        while (treePosX < TILE_MAX_X)
        {
            if (chance())
            {
                index = Random.Range(0, desertTrees.Count);
                GameObject tree = Instantiate(desertTrees[index], tile.transform);

                tree.transform.localScale *= Random.Range(0.25f, 2f);
                Quaternion rotation = tree.transform.rotation;
                tree.transform.rotation = new Quaternion(rotation.x, Random.Range(0.0f, 360.0f), rotation.z, rotation.w);

                treePosZ = tile.transform.position.z + treeZPos + Random.Range(0, treeSpacingZ);
                tree.transform.position = new Vector3(treePosX - 500.0f, getPerlinValue(treePosX - 500.0f, treePosZ), treePosZ);
            }

            treePosX += treeSpacingX;
        }
        
    }

    private void MakeCountry(GameObject tile, int tilePosX, int tilePosZ)
    {
        float treePosZ;
        addNoise(tile, tilePosX, tilePosZ);

        Terrain terrain = tile.GetComponent<Terrain>();

        // Instantiates LHS building
        int index = Random.Range(0, countryBuildings.Count);
        GameObject building = Instantiate(countryBuildings[index], tile.transform);
        building.transform.position = new Vector3(leftRoadOffsetX - 500.0f, 0.0f, tile.transform.position.z + buildingZPos);
        building.SetActive(true);

        // Instantiates RHS building
        index = Random.Range(0, countryBuildings.Count);
        building = Instantiate(countryBuildings[index], tile.transform);
        building.transform.position = new Vector3(rightRoadOffsetX - 500.0f, 0.0f, tile.transform.position.z + buildingZPos);
        building.transform.eulerAngles *= REVERSE;
        building.SetActive(true);

        // LHS trees
        float treePosX = leftRoadOffsetX - buildingSizeX - treeSpacingX;
        while (treePosX > TILE_MIN_X)
        {
            if (chance())
            {
                index = Random.Range(0, desertTrees.Count);
                GameObject tree = Instantiate(desertTrees[index], tile.transform);

                tree.transform.localScale *= Random.Range(0.25f, 2f);
                Quaternion rotation = tree.transform.rotation;
                tree.transform.rotation = new Quaternion(rotation.x, Random.Range(0.0f, 360.0f), rotation.z, rotation.w);

                treePosZ = tile.transform.position.z + treeZPos + Random.Range(0, treeSpacingZ);
                tree.transform.position = new Vector3(treePosX - 500.0f, getPerlinValue(treePosX - 500.0f, treePosZ), treePosZ);
            }

            treePosX -= treeSpacingX;
        }

        // RHS trees
        treePosX = rightRoadOffsetX + buildingSizeX + treeSpacingX;
        while (treePosX < TILE_MAX_X)
        {
            if (chance())
            {
                index = Random.Range(0, desertTrees.Count);
                GameObject tree = Instantiate(desertTrees[index], tile.transform);

                tree.transform.localScale *= Random.Range(0.25f, 2f);
                Quaternion rotation = tree.transform.rotation;
                tree.transform.rotation = new Quaternion(rotation.x, Random.Range(0.0f, 360.0f), rotation.z, rotation.w);

                treePosZ = tile.transform.position.z + treeZPos + Random.Range(0, treeSpacingZ);
                tree.transform.position = new Vector3(treePosX - 500.0f, getPerlinValue(treePosX - 500.0f, treePosZ), treePosZ);
            }

            treePosX += treeSpacingX;
        }
        
    }

    private float getPerlinValue(float x, float z)
    {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        int k = 0;
        int l = 0;

        for (int j = 0; j < octaves; j++)
        {
            float sampleX = (x / scale) * frequency;
            float sampleZ = (z / scale) * frequency;
            float perlinValue = -(Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1);
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistance;
            frequency *= lacunarity;
        }

        if (noiseHeight < 0)
        {
            return 0.0f;
        }

        if (x < LHSstopNoise || x > RHSstopNoise)
        {
            return 0.0f;
        }

        return noiseHeight;
    }

    // Randomly returns true or false
    private bool chance()
    {
        return Random.value > 0.5f;
    }

}

