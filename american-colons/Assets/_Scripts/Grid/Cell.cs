using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private Vector2Int position;
    private float[,,] alphaMaps;

    // if set to true, a placed object can be set on this cell
    private bool isConstructible;

    // current placedObjects on this cell.
    // a placed object can be on multiple cell and will
    // therefore hold a reference to all the cells it's attached to
    private List<PlacedObject> placedObjects;

    private const float constructibleRatio = 0.85f;

    public Cell(Vector2Int position, float[,,] alphaMaps)
    {
        this.position = position;
        this.alphaMaps = alphaMaps;
        this.placedObjects = new();

        ComputeIsConstructible();
    }

    public override string ToString()
    {
        return $"cell [{position.x}; {position.y}] - isConstructible: {isConstructible}";
    }

    public bool CanPlaceObject()
    {
        return isConstructible && placedObjects.Count == 0;
    }

    public void AddPlacedObject(PlacedObject placedObject)
    {
        placedObjects.Add(placedObject);
    }

    public void RemovePlacedObject(PlacedObject placedObject)
    {
        placedObjects.Remove(placedObject);
    }


    // Compute the cell constructible feasabelity based on the alphamap (terrain type)
    // and the height map (terrain topology).
    // considers a cell constructible if it's made at more than 90% grass (a0).
    // also considers that the texture are continuous and are not made of small points.
    private void ComputeIsConstructible()
    {
        float a0 = 0.0f;
        int tot = 0;

        for (int k = 0; k < alphaMaps.GetLength(0); k++)
        {
            for (int l = 0; l < alphaMaps.GetLength(1); l++)
            {
                a0 += alphaMaps[k, l, 0];
                tot++;
            }
        }

        float a0Ratio = a0 / tot;
        isConstructible = a0Ratio > constructibleRatio;
    }
}
