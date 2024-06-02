using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    public enum GameState
    {
        Idle = 0, // default behavior, the player is not doing anything special
        Pause = 1, // the game is in pause
        Building = 2, // the user is in the building interface
    }

    public GameState State { get; private set; }


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
        State = GameState.Idle;
    }

    public void SetState(GameState state)
    {
        Debug.LogFormat("swap state from {0} to {1}", State, state);
        State = state;
    }
}
