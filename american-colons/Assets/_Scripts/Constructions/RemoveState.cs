using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveState : IConstructState
{
    private Building highlightBuilding;

    public RemoveState()
    {
    }

    public void EndState()
    {
        GridSystem.Instance.ResetLastDetectedPosition();
        highlightBuilding = null;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        PlacedObject placedObject = GridSystem.Instance.GetPlacedObjectAtGridPosition(gridPosition);
        
        // do nothin if there are no placed object below the mouse
        if (placedObject == null)
            return;

        // remove the object both at grid data level and in
        // the buildings factory
        GridSystem.Instance.RemoveObjectAt(gridPosition);
        highlightBuilding = null;
    }

    public void UpdateState(Vector3Int gridPosition, float scrollValue)
    {
        PlacedObject placedObject = GridSystem.Instance.GetPlacedObjectAtGridPosition(gridPosition);
        if (placedObject == null || !(placedObject is Building))
            HighlightPlacedObject(null);

        Building building = (Building)placedObject;
        HighlightPlacedObject(building);
    }

    private void HighlightPlacedObject(Building building)
    {
        if (building == null)
        {
            if (highlightBuilding != null)
                highlightBuilding.HideSelection();

            highlightBuilding = null;
            return;
        }
        building.ShowSelection();
        if (highlightBuilding != null && highlightBuilding.UniqueId != building.UniqueId)
            highlightBuilding.HideSelection();

        highlightBuilding = building;
    }
}
