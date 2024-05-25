using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(
        Vector3Int position,
        Vector2Int size,
        int id,
        int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(position, size);
        PlacementData data = new PlacementData(positionsToOccupy, id, placedObjectIndex);

        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"dictionary already contains this cell position: {pos}");
            placedObjects[pos] = data; 
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int position, Vector2Int size)
    {
        List<Vector3Int> returnVals = new();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                returnVals.Add(position + new Vector3Int(x, 0, y));
            }
        }

        return returnVals;
    }

    public bool CanPlaceObjectAt(Vector3Int position, Vector2Int size)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(position, size);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }

        return true;
    }
}

// This can be a road, a building, a forest, a rock...
public class PlacementData
{
    public List<Vector3Int> occupiedPositions;


    public int ID { get; private set; }
    // Add an enum type later
    public int PlacedObjectIndex { get; private set; }


    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}