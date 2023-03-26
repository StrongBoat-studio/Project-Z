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
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.PauseMenu.Pause.performed -= OnPauseMenuToggle;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _pauseMenu.gameObject.SetActive(newGameState == GameStateManager.GameState.Paused);

        if(newGameState == GameStateManager.GameState.Inventory) _playerInput.PauseMenu.Disable();
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
        StartCoroutine(ExitGame());
    }

    private IEnumerator ExitGame()
    {
        yield return new WaitUntil( () => 
            GameStateManager.Instance != null &&
            AudioManager.Instance != null
        );

        // Load/Unload scenes
        Queue<int> ops = new Queue<int>();
        
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(
                SceneManager.GetSceneAt(i).buildIndex == (int)SceneRegister.Scenes.GameManagers ||
                SceneManager.GetSceneAt(i).buildIndex == (int)SceneRegister.Scenes.Player
            ) continue;
            ops.Enqueue(SceneManager.GetSceneAt(i).buildIndex);
        }

        while (ops.Count > 0)
        {
            AsyncOperation aop;
            int buildIndex = ops.Dequeue();
            aop = SceneManager.UnloadSceneAsync(buildIndex);

            yield return new WaitUntil(() => aop.isDone == true);
        }

        AsyncOperation loadMenu = SceneManager.LoadSceneAsync((int)SceneRegister.Scenes.MainMenu, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadMenu.isDone == true);

        AudioManager.Instance.CleanUp();
        GameStateManager.Instance.ResetGameStateStack();

        AsyncOperation unloadPlayer = SceneManager.UnloadSceneAsync((int)SceneRegister.Scenes.Player);
        yield return new WaitUntil(() => unloadPlayer.isDone == true);
        Debug.Log("Exit finished");
    }
}
