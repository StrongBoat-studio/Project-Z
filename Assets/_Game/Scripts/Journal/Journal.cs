using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Journal : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _isOpen = false;

    [SerializeField] private RectTransform _journal;   

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Journal.Enable();
        _playerInput.Journal.Open.performed += OnJournalOpen;
        _playerInput.Journal.ExclusiveClose.performed += OnJournalExculusiveClose;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        _playerInput.Journal.Open.performed -= OnJournalOpen;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnJournalOpen(InputAction.CallbackContext context)
    {
        if(_isOpen == true) GameStateManager.Instance.ResetLastState();
        else GameStateManager.Instance.SetState(GameStateManager.GameState.Journal);
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        //_journal.gameObject.SetActive(newGameState == GameStateManager.GameState.Journal);
        if(newGameState == GameStateManager.GameState.Journal && _isOpen == false)
        {
            _journal.DOAnchorPosX(0f, .25f, true);
            _isOpen = true;
        }
        else if(newGameState != GameStateManager.GameState.Journal && _isOpen == true)
        {
            _journal.DOAnchorPosX(
                -_journal.rect.width - (_journal.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width - _journal.rect.width) / 2, 
                .25f, 
                true
            );
            _isOpen = false;
        }
        
        if(
            newGameState == GameStateManager.GameState.Paused ||
            newGameState == GameStateManager.GameState.Crafting ||
            newGameState == GameStateManager.GameState.Dialogue
        )
        {
            _playerInput.Journal.Disable();
        }
        else _playerInput.Journal.Enable();
    }
    
    private void OnJournalExculusiveClose(InputAction.CallbackContext context)
    {
        if(_isOpen == true) GameStateManager.Instance.ResetLastState();
    }
}
