using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceState : IConstructState
{
    private GridSystem gridSystem;
    private PreviewSystem previewSystem;
    private BuildingData buildingData;
    private BuildingsSystem buildingsSystem;

    public PlaceState(
        GridSystem gridSystem,
        PreviewSystem previewSystem,
        BuildingDatabaseSO buildingDatabase,
        int selectedBuilding,
        BuildingsSystem buildingsFactory)
    {
        int selectedBuildingIndex = buildingDatabase.Buildings.FindIndex(b => b.ID == selectedBuilding);
        if (selectedBuildingIndex < 0 || selectedBuildingIndex >= buildingDatabase.Buildings.Count)
        {
            throw new Exception($"No ID found {selectedBuilding}");
        }
        this.buildingData = buildingDatabase.Buildings[selectedBuildingIndex];
        this.gridSystem = gridSystem;
        this.previewSystem = previewSystem;
        this.buildingsSystem = buildingsFactory;

        gridSystem.ShowGridVisualisation();
        previewSystem.StartPlacementPreview(buildingData.Preview, buildingData.Size);
    }

    public void EndState()
    {
        gridSystem.HideGridVisualisation();
        gridSystem.ResetLastDetectedPosition();
        previewSystem.StopPlacementPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!IsPlacementValid(gridPosition))
            return;

        Vector3 position = gridSystem.CellToWorld(gridPosition);
        int idx = buildingsSystem.Build(buildingData.Prefab, position);

        gridSystem.GridData.AddObjectAt(gridPosition, buildingData.Size, buildingData.ID, idx);

        previewSystem.UpdatePlacementPosition(position, false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool isPlacementValid = IsPlacementValid(gridPosition);

        previewSystem.UpdatePlacementPosition(gridSystem.CellToWorld(gridPosition), isPlacementValid);
    }

    private bool IsPlacementValid(Vector3Int gridPosition)
    {
        return gridSystem.GridData.CanPlaceObjectAt(gridPosition, buildingData.Size);
    }
}
