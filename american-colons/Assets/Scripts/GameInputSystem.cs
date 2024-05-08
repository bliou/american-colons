using System;
using System.Collections;
using System.Collections.Generic;
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
}
