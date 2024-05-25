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
    [SerializeField] private GameInputSystem gameInputSystem;
    // mask onto which we apply the ray cast from the camera
    // system to get the grid world position
    [SerializeField] private LayerMask mask;

    // buildings template database
    [SerializeField] private BuildingDatabaseSO buildingDatabase;
    // current building template under construction
    private BuildingData buildingData;

    [SerializeField] private GameObject gridVisualisation;

    // buildings factory is in charge of building a building
    // this will use internal recipe to construct the building
    // step by step
    [SerializeField] private BuildingsFactory buildingsFactory;

    // grid data contains a dictionary of all the occupied positions
    // and is therefore used to check if an object can be built
    private GridData gridData;

    // the preview system is used to show the preview object on the screen
    // before building it
    [SerializeField] private PreviewSystem previewSystem;

    // lastDetectedPosition keeps in cache the last grid position
    // in order to optimize the call in the update methods
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    // Start is called before the first frame update
    private void Start()
    {
        StopPlacement();
        gridData = new GridData();

        gameInputSystem.OnStartBuilding += GameInputSystem_OnStartBuilding;
        gameInputSystem.OnDestroy += GameInputSystem_OnDestroy;
    }

    // Update is called once per frame
    private void Update()
    {
        if (buildingData == null)
            return;

        Vector3Int gridPosition = GetGridCellWorldPosition();
        if (gridPosition == lastDetectedPosition)
            return;

        lastDetectedPosition = gridPosition;

        bool isPlacementValid = IsPlacementValid(gridPosition);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), isPlacementValid); 
    }

    private void StopPlacement()
    {
        buildingData = null;
        gridVisualisation.SetActive(false);
        previewSystem.StopPlacementPreview();
        lastDetectedPosition = Vector3Int.zero;
        gameInputSystem.OnBuild -= GameInputSystem_OnBuild;
        gameInputSystem.OnCancelBuilding -= GameInputSystem_OnCancelBuilding;
    }

    private void GameInputSystem_OnStartBuilding(object sender, GameInputSystem.OnStartBuildingEventArgs e)
    {
        StopPlacement();
        int selectedBuildingIndex = buildingDatabase.Buildings.FindIndex(b => b.ID == e.selectedBuilding);
        if (selectedBuildingIndex < 0 || selectedBuildingIndex >= buildingDatabase.Buildings.Count)
        {
            Debug.LogError($"No ID found {e.selectedBuilding}");
            return;
        }
        buildingData = buildingDatabase.Buildings[selectedBuildingIndex];
        gridVisualisation.SetActive(true);
        previewSystem.StartPlacementPreview(buildingData.Preview, buildingData.Size);
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

        if (!IsPlacementValid(gridPosition))
            return;

        Vector3 position = grid.CellToWorld(gridPosition);
        int idx = buildingsFactory.Build(buildingData.Building, position);

        gridData.AddObjectAt(gridPosition, buildingData.Size, buildingData.ID, idx);

        previewSystem.UpdatePosition(position, false);
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

    private bool IsPlacementValid(Vector3Int gridPosition)
    {
        return gridData.CanPlaceObjectAt(gridPosition, buildingData.Size);
    }
}
