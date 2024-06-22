using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceState : IConstructState
{
    private BuildingPreview buildingPreview;

    public PlaceState(
        BuildingModels buildingModels,
        int selectedBuilding)
    {
        int selectedBuildingIndex = buildingModels.Models.FindIndex(b => b.ID == selectedBuilding);
        if (selectedBuildingIndex < 0 || selectedBuildingIndex >= buildingModels.Models.Count)
        {
            throw new Exception($"No ID found {selectedBuilding}");
        }
        BuildingModel buildingModel = buildingModels.Models[selectedBuildingIndex];
        buildingPreview = new BuildingPreview(buildingModel);

        GridSystem.Instance.ShowGridVisualisation();
    }

    public void EndState()
    {
        GridSystem.Instance.HideGridVisualisation();
        GridSystem.Instance.ResetLastDetectedPosition();
        buildingPreview.Dispose();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!IsPlacementValid(gridPosition))
            return;

        // for now only consider that we can place buildings
        List<Cell> cells = GridSystem.Instance.GetCells(gridPosition, buildingPreview.GetSize());

        Building building = new Building(buildingPreview.DumpPreview(), cells, gridPosition, buildingPreview.GetSize());
        GridSystem.Instance.AddPlacedObject(building);
    }

    public void UpdateState(Vector3Int gridPosition, float scrollValue)
    {
        RotatePreview(scrollValue);

        bool isPlacementValid = IsPlacementValid(gridPosition);

        buildingPreview.UpdatePlacementPosition(GridSystem.Instance.CellToWorld(gridPosition) + buildingPreview.GetRotationOffset(), isPlacementValid);
    }

    private bool IsPlacementValid(Vector3Int gridPosition)
    {
        return GridSystem.Instance.CanPlaceObjectAt(gridPosition, buildingPreview.GetSize());
    }

    private void RotatePreview(float scrollValue) 
    {
        if (scrollValue == 0)
            return;

        if (scrollValue > 0)
        {
            buildingPreview.RotateRight();
        }
        else
        {
            buildingPreview.RotateLeft();
        }
    }
}
