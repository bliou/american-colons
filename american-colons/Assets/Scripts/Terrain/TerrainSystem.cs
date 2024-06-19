using System.Collections.Generic;
using UnityEngine;

public class TerrainSystem: MonoBehaviour
{
    private Vector2Int mapSize;
    private Cell[,] grid;
    private Vector2 cellSize;

    [SerializeField] private Material terrainMaterial;
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private float waterLevel = .4f;

    private void Start()
    {
        mapSize = GameSystem.Instance.GetMapSize();
        cellSize = gridSystem.GetGridCellSize();

        InitGridTerrain();
    }

    private void InitGridTerrain()
    {
        float[,] noiseMap = new float[mapSize.x, mapSize.y];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * cellSize.x + xOffset, y * cellSize.y + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[mapSize.x, mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float xv = x / (float)mapSize.x * 2 - 1;
                float yv = y / (float)mapSize.y * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[mapSize.x, mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }

        DrawTerrainMesh(grid);
        DrawTexture(grid);
    }

    void DrawTerrainMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Cell cell = grid[x, y];

                    Vector3 a = new Vector3(x, 0, y + cellSize.y);
                    Vector3 b = new Vector3(x + cellSize.x, 0, y + cellSize.y);
                    Vector3 c = new Vector3(x, 0, y);
                    Vector3 d = new Vector3(x + cellSize.x, 0, y);
                    Vector2 uvA = new Vector2(x / (float)mapSize.x, y / (float)mapSize.y);
                    Vector2 uvB = new Vector2((x + 1) / (float)mapSize.x, y / (float)mapSize.y);
                    Vector2 uvC = new Vector2(x / (float)mapSize.x, (y + 1) / (float)mapSize.y);
                    Vector2 uvD = new Vector2((x + 1) / (float)mapSize.x, (y + 1) / (float)mapSize.y);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    void DrawTexture(Cell[,] grid)
    {
        Texture2D texture = new Texture2D(mapSize.x, mapSize.y);
        Color[] colorMap = new Color[mapSize.x * mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * mapSize.y + x] = Color.blue;
                else
                    colorMap[y * mapSize.y + x] = Color.green;

            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;
    //    for (int y = 0; y < mapSize.y; y++)
    //    {
    //        for (int x = 0; x < mapSize.x; x++)
    //        {
    //            Cell cell = grid[x, y];
    //            if (cell.isWater)
    //                Gizmos.color = Color.blue;
    //            else
    //                Gizmos.color = Color.green;
    //            Vector3 pos = new Vector3(x-0.5f, 0, y-0.5f);
    //            Gizmos.DrawCube(pos, Vector3.one);
    //        }
    //    }
    //}
}