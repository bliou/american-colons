using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject
{
    public event EventHandler OnHighlight;
    public event EventHandler OnStopHighlighting;

    // cells on to which the placed object is placed
    private List<Cell> cells;

    // grid position of this placed object
    private Vector3Int gridPosition;

    // size of the placed object
    private Vector2Int size;

    // if set to true the placed object is being highlighted
    private bool isHighlighted;

    // unique identifier of this placed object
    public int UniqueId { get; private set; }
    private static int uniqueId = 0;

    public PlacedObject(List<Cell> cells, Vector3Int gridPosition, Vector2Int size)
    {
        this.cells = cells;
        this.gridPosition = gridPosition;
        this.size = size;

        UniqueId = uniqueId++;
    }

    public void RemovePlacedObject()
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

    public void Highlight()
    {
        if (isHighlighted)
            return;

        isHighlighted = true;
        OnHighlight?.Invoke(this, EventArgs.Empty);
    }

    public void StopHighlighting()
    {
        isHighlighted = false;
        OnStopHighlighting?.Invoke(this, EventArgs.Empty);
    }
}
