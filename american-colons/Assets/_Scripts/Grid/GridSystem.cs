using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance { get; private set; }

    // grid class of the unity system.
    // allows us to get the cell from the mouse cursor and
    // other positions easily
    [SerializeField] private Grid grid;

    // cutomGrid that will hold the useful cells data
    // compared to the grid above.
    // this customGrid will help us know if an object can
    // be placed, or to compute the best path to take
    private Cell[,] customGrid;

    // list of all the placed objects on the grid
    private List<PlacedObject> placedObjects;

    // a reference to the terrain
    private Terrain terrain;

    // mask onto which we apply the ray cast from the camera
    // system to get the grid world position
    [SerializeField] private LayerMask mask;

    [SerializeField] private GameObject gridVisualisation;

    // lastDetectedPosition keeps in cache the last grid position
    // in order to optimize the call in the update methods
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public bool IsLastPositionUpdated { get; private set; } = false ;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("instance already exists");
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        placedObjects = new();
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
        // if the raycast did not hit (mouse outside of the terrain), then 
        // keep the last detected position
        Vector3 gridWorldPosition = CameraSystem.Instance.ScreenPointToRay(mask);
        // compare only the x since the vector3 comparison does not work
        if (gridWorldPosition.x == float.NegativeInfinity) {
            return lastDetectedPosition;
        }
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

    public void AddPlacedObject(PlacedObject placedObject)
    {
        placedObjects.Add(placedObject);
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        PlacedObject placedObject = GetPlacedObjectAtGridPosition(gridPosition);
        if (placedObject == null)
            throw new Exception($"no placed object on grid");

        placedObject.RemovePlacedObject();
        placedObjects.Remove(placedObject);
    }

    public List<Cell> GetCells(Vector3Int position, Vector2Int size)
    {
        List<Cell> cells = new();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cellPos = position + new Vector3Int(x, 0, y);
                if (cellPos.x < customGrid.GetLength(0) && cellPos.z < customGrid.GetLength(1))
                    cells.Add(customGrid[cellPos.x, cellPos.z]);
            }
        }

        return cells;
    }

    public PlacedObject GetPlacedObjectAtGridPosition(Vector3Int gridPosition)
    {
        foreach (var placedObject in placedObjects)
        {
            if (placedObject.IsGridPositionOnPlacedObject(gridPosition))
            {
                return placedObject;
            }
        }

        return null;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size)
    {
        // we can't place an object that overlap with the current terrain
        if (gridPosition.x < 0 || gridPosition.z < 0 ||
            gridPosition.x + size.x > customGrid.GetLength(0) ||
            gridPosition.z + size.y > customGrid.GetLength(1))
            return false;

        List<Cell> cellsToOccupy = GetCells(gridPosition, size);
        foreach (var cell in cellsToOccupy)
        {
            if (!cell.CanPlaceObject())
                return false;
        }

        return true;
    }
}
