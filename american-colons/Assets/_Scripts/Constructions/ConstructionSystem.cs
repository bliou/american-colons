using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSystem
{
    // buildings template database
    private BuildingModels buildingModels;

    private IConstructState constructState;

    public ConstructionSystem(BuildingModels buildingModels)
    {
        this.buildingModels = buildingModels;

        GameInputSystem.Instance.OnStartBuilding += GameInputSystem_OnStartBuilding;
        GameInputSystem.Instance.OnStartDestruction += GameInputSystem_OnStartDestruction;
    }

    public void Update(float deltaTime)
    {
        float scrollValue = GameInputSystem.Instance.GetScrollValue();
        if (constructState == null || 
            (!GridSystem.Instance.IsLastPositionUpdated && (scrollValue == 0 || GameInputSystem.Instance.ControlIsBeingPressed)))
            return;

        Vector3Int gridPosition = GridSystem.Instance.GetGridCellWorldPosition();
        constructState.UpdateState(gridPosition, scrollValue);
    }

    private void Cancel()
    {
        GameInputSystem.Instance.OnBuildDestroy -= GameInputSystem_OnBuildDestroy;
        GameInputSystem.Instance.OnCancel -= GameInputSystem_OnCancel;
    
        if (constructState != null)
            constructState.EndState();
        constructState = null;

        GameSystem.Instance.SetState(GameState.Idle);
    }

    private void GameInputSystem_OnStartBuilding(object sender, GameInputSystem.OnStartBuildingEventArgs e)
    {
        Cancel();

        GameSystem.Instance.SetState(GameState.Constructing);

        // add the temporary binding to actually place or cancel the building
        GameInputSystem.Instance.OnBuildDestroy += GameInputSystem_OnBuildDestroy;
        GameInputSystem.Instance.OnCancel += GameInputSystem_OnCancel;

        constructState = new PlaceState(
            buildingModels, 
            e.selectedBuilding);


        Vector3Int gridPosition = GridSystem.Instance.GetGridCellWorldPosition();
        constructState.UpdateState(gridPosition, 0f);
    }

    private void GameInputSystem_OnStartDestruction(object sender, System.EventArgs e)
    {
        Cancel();

        GameSystem.Instance.SetState(GameState.Destroying);

        // add the temporary binding to actually place or cancel the building
        GameInputSystem.Instance.OnBuildDestroy += GameInputSystem_OnBuildDestroy;
        GameInputSystem.Instance.OnCancel += GameInputSystem_OnCancel;

        constructState = new RemoveState();

        Vector3Int gridPosition = GridSystem.Instance.GetGridCellWorldPosition();
        constructState.UpdateState(gridPosition, 0f);
    }

    private void GameInputSystem_OnBuildDestroy(object sender, System.EventArgs e)
    {
        Vector3Int gridPosition = GridSystem.Instance.GetGridCellWorldPosition();
        constructState.OnAction(gridPosition);
        Cancel();
    }

    private void GameInputSystem_OnCancel(object sender, System.EventArgs e)
    {
        Cancel();
    }
}
