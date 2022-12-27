using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Paused
    }

    public static GameStateManager Instance { get; private set; }

    private GameState _currentGameState;

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == _currentGameState)
            return;
 
        _currentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }

    public GameState GetState()
    {
        return _currentGameState;
    }
}
