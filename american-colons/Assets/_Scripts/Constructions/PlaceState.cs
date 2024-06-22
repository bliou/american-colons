using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceState : IConstructState
{
    private BuildingModel buildingModel;

    private float previewYOffset = 0.06f;
    private GameObject previewObject;

    public PlaceState(
        BuildingModels buildingModels,
        int selectedBuilding)
    {
        int selectedBuildingIndex = buildingModels.Models.FindIndex(b => b.ID == selectedBuilding);
        if (selectedBuildingIndex < 0 || selectedBuildingIndex >= buildingModels.Models.Count)
        {
            throw new Exception($"No ID found {selectedBuilding}");
        }
        buildingModel = buildingModels.Models[selectedBuildingIndex].Clone();

        GridSystem.Instance.ShowGridVisualisation();
        previewObject = GameSystem.Instance.InstantiateGO(buildingModel.Preview);
    }

    public void EndState()
    {
        GridSystem.Instance.HideGridVisualisation();
        GridSystem.Instance.ResetLastDetectedPosition();
        GameSystem.Instance.DestroyGO(previewObject);
        previewObject = null;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!IsPlacementValid(gridPosition))
            return;

        // for now only consider that we can place buildings
        Vector3 position = GridSystem.Instance.CellToWorld(gridPosition) + buildingModel.GetRotationOffset();
        List<Cell> cells = GridSystem.Instance.GetCells(gridPosition, buildingModel.GetSize());
        GameObject gameObject = GameSystem.Instance.InstantiateGO(buildingModel.Prefab);
        gameObject.transform.position = position;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, buildingModel.GetRotationAngle(), 0));

        Building building = new Building(gameObject, cells, gridPosition, buildingModel.GetSize());
        GridSystem.Instance.AddPlacedObject(building);

        UpdatePlacementPosition(position, false);
    }

    public void UpdateState(Vector3Int gridPosition, float scrollValue)
    {
        RotatePreview(scrollValue);

        bool isPlacementValid = IsPlacementValid(gridPosition);

        UpdatePlacementPosition(GridSystem.Instance.CellToWorld(gridPosition) + buildingModel.GetRotationOffset(), isPlacementValid);
    }

    private bool IsPlacementValid(Vector3Int gridPosition)
    {
        return GridSystem.Instance.CanPlaceObjectAt(gridPosition, buildingModel.GetSize());
    }

    private void RotatePreview(float scrollValue) 
    {
        if (scrollValue == 0)
            return;

        if (scrollValue > 0)
        {
            buildingModel.RotateRight();
        }
        else
        {
            buildingModel.RotateLeft();
        }
        previewObject.transform.rotation = Quaternion.Euler(new Vector3(0, buildingModel.GetRotationAngle(), 0));
    }

    private void UpdatePlacementPosition(Vector3 position, bool isPlacementValid)
    {
        Color c = isPlacementValid ? Color.green : Color.red;
        c.a = 0.5f;

        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                material.color = c;
            }
        }

        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
}
