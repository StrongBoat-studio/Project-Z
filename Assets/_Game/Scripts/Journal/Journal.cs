using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Journal : MonoBehaviour
{
    private PlayerInput _playerInput;

    [SerializeField] private Transform _journal;   

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Journal.Enable();
        _playerInput.Journal.Open.performed += OnJournalOpen;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        _playerInput.Journal.Open.performed -= OnJournalOpen;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnJournalOpen(InputAction.CallbackContext context)
    {
        if(_journal.gameObject.activeSelf) GameStateManager.Instance.ResetLastState();
        else GameStateManager.Instance.SetState(GameStateManager.GameState.Journal);
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if(newGameState == GameStateManager.GameState.Journal)
        {
            if(_journal.gameObject.activeSelf == false) _journal.gameObject.SetActive(true);
        }
        else if(newGameState != GameStateManager.GameState.Journal && _journal.gameObject.activeSelf == true)
        {
            _journal.gameObject.SetActive(false);
        }
    }
}
