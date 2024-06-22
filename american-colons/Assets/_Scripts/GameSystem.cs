using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum GameState
{
    Idle = 0, // default behavior, the player is not doing anything special
    Pause = 1, // the game is in pause
    Building = 2, // the user is in the building interface
}

public class GameSystem : MonoBehaviour
{

    [SerializeField] private BuildingModels buildingModels;

    public static GameSystem Instance { get; private set; }

    public GameState State { get; private set; }

    private ConstructionSystem constructionSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("instance already exists");
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        constructionSystem = new ConstructionSystem(buildingModels);
        State = GameState.Idle;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        constructionSystem.Update(deltaTime);
    }

    public void SetState(GameState state)
    {
        if (State == state)
            return;

        Debug.LogFormat("swap state from {0} to {1}", State, state);
        State = state;
    }

    public GameObject InstantiateGO(GameObject go)
    {
        return Instantiate(go);
    }

    public void DestroyGO(GameObject go)
    {
        Destroy(go);
    }
}
