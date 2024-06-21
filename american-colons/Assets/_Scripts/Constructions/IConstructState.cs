using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConstructState
{
    public void EndState();
    public void OnAction(Vector3Int gridPosition);
    public void UpdateState(Vector3Int gridPosition, float scrollValue);
}
