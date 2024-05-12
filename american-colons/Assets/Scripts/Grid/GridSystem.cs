using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private GameInputSystem gameInputSystem;
    [SerializeField] private LayerMask mask;

    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    private int selectedBuildingIndex = -1;

    [SerializeField] private GameObject gridVisualisation;

    private bool drawCellIndicator;

    // Start is called before the first frame update
    private void Start()
    {
        StopPlacement();
        gameInputSystem.OnStartBuilding += GameInputSystem_OnStartBuilding;
        gameInputSystem.OnDestroy += GameInputSystem_OnDestroy;
    }

    // Update is called once per frame
    private void Update()
    {
        DrawCellIndicator();
    }

    private void StopPlacement()
    {
        selectedBuildingIndex = -1;
        gridVisualisation.SetActive(false);
        drawCellIndicator = false;
        gameInputSystem.OnBuild -= GameInputSystem_OnBuild;
        gameInputSystem.OnCancelBuilding -= GameInputSystem_OnCancelBuilding;
    }

    private void GameInputSystem_OnStartBuilding(object sender, GameInputSystem.OnStartBuildingEventArgs e)
    {
        StopPlacement();
        selectedBuildingIndex = buildingDatabase.Buildings.FindIndex(b => b.ID == e.selectedBuilding);
        if (selectedBuildingIndex < 0 || selectedBuildingIndex >= buildingDatabase.Buildings.Count)
        {
            Debug.LogError($"No ID found {e.selectedBuilding}");
            return;
        }
        gridVisualisation.SetActive(true);
        drawCellIndicator = true;
        gameInputSystem.OnBuild += GameInputSystem_OnBuild;
        gameInputSystem.OnCancelBuilding += GameInputSystem_OnCancelBuilding;
    }

    private void GameInputSystem_OnCancelBuilding(object sender, System.EventArgs e)
    {
        StopPlacement();
    }

    private void GameInputSystem_OnBuild(object sender, System.EventArgs e)
    {
        Vector3Int gridPosition = GetGridCellWorldPosition();
        GameObject newBuilding = Instantiate(buildingDatabase.Buildings[selectedBuildingIndex].Prefab);
        newBuilding.transform.position = grid.CellToWorld(gridPosition);

        StopPlacement();
    }

    private void GameInputSystem_OnDestroy(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private Vector3Int GetGridCellWorldPosition()
    {
        Vector3 gridWorldPosition = cameraSystem.ScreenPointToRay(mask);
        Vector3Int gridPosition = grid.WorldToCell(gridWorldPosition);
        gridPosition.y = 0;

        return gridPosition;
    }

    private void DrawCellIndicator()
    {
        if (!drawCellIndicator)
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
