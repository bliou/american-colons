using System;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    public event EventHandler<OnStartBuildingEventArgs> OnStartBuilding;
    public event EventHandler OnCancelBuilding;
    public event EventHandler OnBuild;
    public event EventHandler OnDestroy;

    public class OnStartBuildingEventArgs : EventArgs {
        public int selectedBuilding;
    }


    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Building.BuildingSelection.performed += BuildingSelection_performed;
        playerInputActions.Building.Cancel.performed += Cancel_performed;
        playerInputActions.Building.Build.performed += Build_performed;
        playerInputActions.Building.Destroy.performed += Destroy_performed;


        playerInputActions.Camera.Enable();
        playerInputActions.Building.Enable();
    }

    private void Cancel_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCancelBuilding?.Invoke(this, EventArgs.Empty);
    }

    private void BuildingSelection_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStartBuilding?.Invoke(this, new OnStartBuildingEventArgs { 
            selectedBuilding = (int)obj.ReadValue<float>()
        });
    }

    private void Build_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnBuild?.Invoke(this, EventArgs.Empty);
    }

    private void Destroy_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDestroy?.Invoke(this, EventArgs.Empty);
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
        return playerInputActions.Camera.Position.ReadValue<Vector2>();
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
