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

    private Vector3Int GetGridCellWorldPosition()
    {
        Ray ray = cameraSystem.ScreenPointToRay();
        RaycastHit hit;
        Vector3 gridWorldPosition = Vector3.zero;
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            gridWorldPosition = hit.point;
        }
        Vector3Int gridPosition = grid.WorldToCell(gridWorldPosition);
        gridPosition.y = 0;

        return gridPosition;
    }

    private void Draw()
    {
        if (!drawGrid)
            return;

        Vector3Int topLeftCorner = GetGridCellWorldPosition();
        Vector3Int topRightCorner = new Vector3Int(topLeftCorner.x+1, topLeftCorner.y, topLeftCorner.z);
        Vector3Int bottomRightCorner = new Vector3Int(topLeftCorner.x + 1, topLeftCorner.y, topLeftCorner.z + 1);
        Vector3Int bottomLeftCorner = new Vector3Int(topLeftCorner.x, topLeftCorner.y, topLeftCorner.z + 1);

        Debug.DrawLine(topLeftCorner, topRightCorner, Color.white);
        Debug.DrawLine(topLeftCorner, bottomLeftCorner, Color.white);
        Debug.DrawLine(topRightCorner, bottomRightCorner, Color.white);
        Debug.DrawLine(bottomLeftCorner, bottomRightCorner, Color.white);
    }
}
