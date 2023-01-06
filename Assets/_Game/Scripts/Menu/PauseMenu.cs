using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Transform _pauseMenu;
    private PlayerInput _playerInput;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    
        _playerInput = new PlayerInput();
        _playerInput.PauseMenu.Enable();
        _playerInput.PauseMenu.Pause.performed += OnPauseMenuToggle;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        _playerInput.PauseMenu.Pause.performed -= OnPauseMenuToggle;
    }

    private void Update()
    {

    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _pauseMenu.gameObject.SetActive(newGameState == GameStateManager.GameState.Paused);
    }

    private void OnPauseMenuToggle(InputAction.CallbackContext context)
    {
        GameStateManager.GameState newGameState = GameStateManager.Instance.GetState() == GameStateManager.GameState.Gameplay
            ? GameStateManager.GameState.Paused
            : GameStateManager.GameState.Gameplay;

        GameStateManager.Instance.SetState(newGameState);
    }

    public void Resume()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Gameplay);
    }

    public void Options()
    {
        SceneManager.LoadSceneAsync((int)SceneRegister.Scenes.OptionsMenu, LoadSceneMode.Additive);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync((int)SceneRegister.Scenes.MainMenu);
    }
}
