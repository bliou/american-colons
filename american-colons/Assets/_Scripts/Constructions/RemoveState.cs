using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveState : IConstructState
{
    private GridSystem gridSystem;
    private PreviewSystem previewSystem;
    private BuildingsSystem buildingsFactory;

    public RemoveState(
        GridSystem gridSystem,
        PreviewSystem previewSystem,
        BuildingsSystem buildingsFactory)
    {
        this.gridSystem = gridSystem;
        this.previewSystem = previewSystem;
        this.buildingsFactory = buildingsFactory;
    }

    public void EndState()
    {
        gridSystem.ResetLastDetectedPosition();
        previewSystem.StopPlacementPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        int placedObjectIdx = gridSystem.GridData.GetObjectIndexAt(gridPosition);
        
        // do nothin if there are no placed object below the mouse
        if (placedObjectIdx == -1)
            return;

        // remove the object both at grid data level and in
        // the buildings factory
        gridSystem.GridData.RemoveObjectAt(gridPosition);
        buildingsFactory.RemoveAt(placedObjectIdx);
    }

    public void UpdateState(Vector3Int gridPosition, float scrollValue)
    {
        Building placedObject = GetPlacedObject(gridPosition);
        previewSystem.HighlightPlacedObject(placedObject);
    }

    private Building GetPlacedObject(Vector3Int gridPosition)
    {
        int placedObjectIdx = gridSystem.GridData.GetObjectIndexAt(gridPosition);
        if (placedObjectIdx == -1)
            return null;

        return buildingsFactory.GetBuilding(placedObjectIdx);
    }
}
