using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject
{
    // cells on to which the placed object is placed
    private List<Cell> cells;

    // grid position of this placed object
    protected Vector3Int gridPosition;

    // size of the placed object
    protected Vector2Int size;

    // unique identifier of this placed object
    public int UniqueId { get; private set; }
    private static int uniqueId = 0;

    public PlacedObject(List<Cell> cells, Vector3Int gridPosition, Vector2Int size)
    {
        foreach (var cell in cells)
        {
            if (!cell.CanPlaceObject())
                throw new Exception($"cannot place object on cell: {cell}");
            cell.AddPlacedObject(this);
        }

        this.cells = cells;
        this.gridPosition = gridPosition;
        this.size = size;

        UniqueId = uniqueId++;
    }

    public virtual void RemovePlacedObject()
    {
        foreach (var cell in cells)
        {
            cell.RemovePlacedObject(this);
        }
    }

    public bool IsGridPositionOnPlacedObject(Vector3Int gridPosition)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (gridPosition == this.gridPosition + new Vector3Int(i, 0, j))
                    return true;
            }
        }

        return false;
    }
}
