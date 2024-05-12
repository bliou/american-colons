using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private LayerMask mask;

    [SerializeField] private bool drawGrid;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        Draw();
    }

    private Vector3 GetGridCellWorldPosition()
    {
        Vector3 gridWorldPosition = cameraSystem.ScreenPointToRay(mask);
        Vector3Int gridPosition = grid.WorldToCell(gridWorldPosition);
        gridPosition.y = 0;

        return gridPosition;
    }

    private void Draw()
    {
        if (!drawGrid)
            return;

        Vector3 topLeftCorner = GetGridCellWorldPosition();
        topLeftCorner.y = 0.1f;
        Vector3 topRightCorner = new Vector3(topLeftCorner.x+1, topLeftCorner.y, topLeftCorner.z);
        Vector3 bottomRightCorner = new Vector3(topLeftCorner.x + 1, topLeftCorner.y, topLeftCorner.z + 1);
        Vector3 bottomLeftCorner = new Vector3(topLeftCorner.x, topLeftCorner.y, topLeftCorner.z + 1);

        Debug.DrawLine(topLeftCorner, topRightCorner, Color.white);
        Debug.DrawLine(topLeftCorner, bottomLeftCorner, Color.white);
        Debug.DrawLine(topRightCorner, bottomRightCorner, Color.white);
        Debug.DrawLine(bottomLeftCorner, bottomRightCorner, Color.white);
    }
}
