using UnityEngine;

public class Cell
{
    private Vector2Int position;
    private float[,,] alphaMaps;
    private bool isConstructible;

    private const float constructibleRatio = 0.85f;

    public Cell(Vector2Int position, float[,,] alphaMaps)
    {
        this.position = position;
        this.alphaMaps = alphaMaps;

        ComputeIsConstructible();
    }

    public override string ToString()
    {
        return $"cell [{position.x}; {position.y}] - isConstructible: {isConstructible}";
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
