using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSystem : MonoBehaviour
{
    [SerializeField] private GameInputSystem gameInputSystem;

    // buildings template database
    [SerializeField] private BuildingDatabaseSO buildingDatabase;

    // buildings factory is in charge of building a building
    // this will use internal recipe to construct the building
    // step by step
    [SerializeField] private BuildingsFactory buildingsFactory;

    // the preview system is used to show the preview object on the screen
    // before building it
    [SerializeField] private PreviewSystem previewSystem;

    // the grid system is used to know the current topology
    // of the grid 
    [SerializeField] private GridSystem gridSystem;

    private IConstructState constructState;

    private void Start()
    {
        StopPlacement();

        gameInputSystem.OnStartBuilding += GameInputSystem_OnStartBuilding;
        gameInputSystem.OnStartDestruction += GameInputSystem_OnDestroy;
    }

    private void Update()
    {
        if (constructState == null || gridSystem.IsLastPositionUpdated)
            return;

        Vector3Int gridPosition = gridSystem.GetGridCellWorldPosition();
        constructState.UpdateState(gridPosition);
    }

    private void StopPlacement()
    {
        gameInputSystem.OnBuild -= GameInputSystem_OnBuild;
        gameInputSystem.OnCancel -= GameInputSystem_OnCancelBuilding;
    
        if (constructState != null)
            constructState.EndState();
        constructState = null;
    }

    private void GameInputSystem_OnStartBuilding(object sender, GameInputSystem.OnStartBuildingEventArgs e)
    {
        StopPlacement();

        // add the temporary binding to actually place or cancel the building
        gameInputSystem.OnBuild += GameInputSystem_OnBuild;
        gameInputSystem.OnCancel += GameInputSystem_OnCancelBuilding;

        constructState = new PlaceState(
            gridSystem, 
            previewSystem, 
            buildingDatabase, 
            e.selectedBuilding,
            buildingsFactory);
    }


    private void GameInputSystem_OnBuild(object sender, System.EventArgs e)
    {
        Vector3Int gridPosition = gridSystem.GetGridCellWorldPosition();
        constructState.OnAction(gridPosition);
    }

    private void GameInputSystem_OnDestroy(object sender, System.EventArgs e)
    {
        Vector3Int gridPosition = gridSystem.GetGridCellWorldPosition();
    }


    private void GameInputSystem_OnCancelBuilding(object sender, System.EventArgs e)
    {
        StopPlacement();
    }
}
