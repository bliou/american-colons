using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    // grid class of the unity system.
    // allows us to use 
    [SerializeField] private Grid grid;

    private Cell[,] customGrid;

    // the camera system is used to calculate the grid
    // world position
    [SerializeField] private CameraSystem cameraSystem;
    // mask onto which we apply the ray cast from the camera
    // system to get the grid world position
    [SerializeField] private LayerMask mask;

    [SerializeField] private GameObject gridVisualisation;

    // grid data contains a dictionary of all the occupied positions
    // and is therefore used to check if an object can be built
    public GridData GridData { get; private set; }

    // a reference to the terrain
    private Terrain terrain;

    // lastDetectedPosition keeps in cache the last grid position
    // in order to optimize the call in the update methods
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public bool IsLastPositionUpdated { get; private set; } = false ;

    // Start is called before the first frame update
    private void Start()
    {
        GridData = new GridData();
        terrain = Terrain.activeTerrain;

        int terrainWidth = (int)terrain.terrainData.size.x;
        int terrainHeight = (int)terrain.terrainData.size.z;
        customGrid = new Cell[terrainWidth, terrainHeight];

        for (int i = 0; i < terrainHeight; i++)
        {
            for (int j = 0; j < terrainWidth; j++)
            {
                // get the splat map position based on the grid index
                Vector3 splatMapPosition = new Vector3(j / terrain.terrainData.size.x, 0, i / terrain.terrainData.size.z);
                int x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
                int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);

                // get the splat map width and height.
                // one cell is might be bigger than one alphamap. Therefore we want to grab all the alphamap
                // that are presents in our cell to determine what our cell look like.
                // Take two as a max so that we have the alphamaps at the beginning and at the end of our cell
                int width = Mathf.Max(2, Mathf.FloorToInt(terrain.terrainData.alphamapWidth / terrain.terrainData.size.x));
                int height = Mathf.Max(2, Mathf.FloorToInt(terrain.terrainData.alphamapHeight / terrain.terrainData.size.z));

                float[,,] alphaMaps = terrain.terrainData.GetAlphamaps(x, z, width, height);
                customGrid[j, i] = new Cell(new Vector2Int(j, i), alphaMaps);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3Int gridPosition = GetGridCellWorldPosition();
        if (gridPosition == lastDetectedPosition)
        {
            IsLastPositionUpdated = false;
            return;
        }
        IsLastPositionUpdated = true;
        lastDetectedPosition = gridPosition;
    }

    public Vector3Int GetGridCellWorldPosition()
    {
        Vector3 gridWorldPosition = cameraSystem.ScreenPointToRay(mask);
        Vector3Int gridPosition = grid.WorldToCell(gridWorldPosition);
        gridPosition.y = 0;

        return gridPosition;
    }

    public Vector3 CellToWorld(Vector3Int cellWorldPosition)
    {
        return grid.CellToWorld(cellWorldPosition);
    }

    public void ShowGridVisualisation()
    {
        gridVisualisation.SetActive(true);
    }

    public void HideGridVisualisation()
    {
        gridVisualisation.SetActive(false);
    }

    public void ResetLastDetectedPosition()
    {
        lastDetectedPosition = Vector3Int.zero;
    }
}
