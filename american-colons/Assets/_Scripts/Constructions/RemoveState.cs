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
        previewSystem.HighlightPlacedObject(null);
    }

    public void OnAction(Vector3Int gridPosition)
    {
        PlacedObject placedObject = gridSystem.GetPlacedObjectAtGridPosition(gridPosition);
        
        // do nothin if there are no placed object below the mouse
        if (placedObject == null)
            return;

        // remove the object both at grid data level and in
        // the buildings factory
        gridSystem.RemoveObjectAt(gridPosition);
        buildingsFactory.RemoveBuilding(placedObject.UniqueId);
    }

    public void UpdateState(Vector3Int gridPosition, float scrollValue)
    {
        PlacedObject placedObject = gridSystem.GetPlacedObjectAtGridPosition(gridPosition);
        previewSystem.HighlightPlacedObject(placedObject);
    }
}
