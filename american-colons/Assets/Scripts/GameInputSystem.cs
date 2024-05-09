using System;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Camera.Enable();
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        return playerInputActions.Camera.Movement.ReadValue<Vector2>();
    }

    public float GetRotationDirection()
    {
        return playerInputActions.Camera.Rotation.ReadValue<float>();
    }

    public Vector2 GetMousePosition()
    {
        return playerInputActions.Camera.ScrollEdge.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDragDeltaVector()
    {
        return playerInputActions.Camera.MouseDrag.ReadValue<Vector2>();
    }

    public float GetScrollValue()
    {
        float scrollValue =  playerInputActions.Camera.Scroll.ReadValue<float>();
        if (scrollValue > 0)
            return 1;
        else if (scrollValue < 0)
            return -1;
        else 
            return 0;
    }
}
