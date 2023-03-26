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

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _pauseMenu.gameObject.SetActive(newGameState == GameStateManager.GameState.Paused);

        if(
            newGameState == GameStateManager.GameState.Inventory ||
            newGameState == GameStateManager.GameState.Crafting
        ) _playerInput.PauseMenu.Disable();
        else _playerInput.PauseMenu.Enable();
    }

    private void OnPauseMenuToggle(InputAction.CallbackContext context)
    {
        if(_pauseMenu.gameObject.activeSelf) GameStateManager.Instance.ResetLastState();
        else GameStateManager.Instance.SetState(GameStateManager.GameState.Paused);
    }

    public void Resume()
    {
        GameStateManager.Instance.ResetLastState();
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
