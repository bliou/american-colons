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
    [SerializeField] private BuildingsSystem buildingsSystem;

    // the preview system is used to show the preview object on the screen
    // before building it
    [SerializeField] private PreviewSystem previewSystem;

    // the grid system is used to know the current topology
    // of the grid 
    [SerializeField] private GridSystem gridSystem;

    private IConstructState constructState;

    private void Start()
    {
        Cancel();

        gameInputSystem.OnStartBuilding += GameInputSystem_OnStartBuilding;
        gameInputSystem.OnStartDestruction += GameInputSystem_OnStartDestruction;
    }

    private void Update()
    {
        float scrollValue = gameInputSystem.GetScrollValue();
        if (constructState == null || 
            (!gridSystem.IsLastPositionUpdated && (scrollValue == 0 || gameInputSystem.ControlIsBeingPressed)))
            return;

        Vector3Int gridPosition = gridSystem.GetGridCellWorldPosition();
        constructState.UpdateState(gridPosition, scrollValue);
    }

    private void Cancel()
    {
        gameInputSystem.OnBuildDestroy -= GameInputSystem_OnBuildDestroy;
        gameInputSystem.OnCancel -= GameInputSystem_OnCancel;
    
        if (constructState != null)
            constructState.EndState();
        constructState = null;

        GameSystem.Instance.SetState(GameState.Idle);
    }

    private void GameInputSystem_OnStartBuilding(object sender, GameInputSystem.OnStartBuildingEventArgs e)
    {
        Cancel();

        GameSystem.Instance.SetState(GameState.Building);

        // add the temporary binding to actually place or cancel the building
        gameInputSystem.OnBuildDestroy += GameInputSystem_OnBuildDestroy;
        gameInputSystem.OnCancel += GameInputSystem_OnCancel;

        constructState = new PlaceState(
            gridSystem, 
            previewSystem, 
            buildingDatabase, 
            e.selectedBuilding,
            buildingsSystem);


        Vector3Int gridPosition = gridSystem.GetGridCellWorldPosition();
        constructState.UpdateState(gridPosition, 0f);
    }

    private void GameInputSystem_OnStartDestruction(object sender, System.EventArgs e)
    {
        Cancel();

        GameSystem.Instance.SetState(GameState.Building);

        // add the temporary binding to actually place or cancel the building
        gameInputSystem.OnBuildDestroy += GameInputSystem_OnBuildDestroy;
        gameInputSystem.OnCancel += GameInputSystem_OnCancel;

        constructState = new RemoveState(
            gridSystem,
            previewSystem,
            buildingsSystem);
    }

    private void GameInputSystem_OnBuildDestroy(object sender, System.EventArgs e)
    {
        Vector3Int gridPosition = gridSystem.GetGridCellWorldPosition();
        constructState.OnAction(gridPosition);
    }

    private void GameInputSystem_OnCancel(object sender, System.EventArgs e)
    {
        Cancel();
    }
}
