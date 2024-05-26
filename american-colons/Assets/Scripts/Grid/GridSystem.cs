using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    // grid class of the unity system.
    // allows us to use 
    [SerializeField] private Grid grid;
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

    // lastDetectedPosition keeps in cache the last grid position
    // in order to optimize the call in the update methods
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    public bool IsLastPositionUpdated { get; private set; } = false ;

    // Start is called before the first frame update
    private void Start()
    {
        GridData = new GridData();
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
