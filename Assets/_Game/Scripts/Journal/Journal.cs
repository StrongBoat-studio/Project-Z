using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Journal : MonoBehaviour
{
    private PlayerInput _playerInput;
    public bool IsOpen { get; private set; } = false;

    [SerializeField] private RectTransform _journal; 
    [SerializeField] private RectTransform _lifeMonitor;  

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
        if (IsOpen == true) GameStateManager.Instance.ResetLastState();
        else GameStateManager.Instance.SetState(GameStateManager.GameState.Journal);
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if(newGameState == GameStateManager.GameState.Journal && IsOpen == false)
        {
            _journal.DOAnchorPosX(0f, .25f, true);
            IsOpen = true;

            if(_lifeMonitor.GetComponent<UI_LifeMonitor>().gameObject.activeSelf)
            {
                _lifeMonitor.GetComponent<UI_LifeMonitor>().StartAudio();
            }
        }
        else if(newGameState != GameStateManager.GameState.Journal && IsOpen == true)
        {
            _journal.DOAnchorPosX(
                -_journal.rect.width - (_journal.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width - _journal.rect.width) / 2, 
                .25f, 
                true
            );
            IsOpen = false;

            if(_lifeMonitor.GetComponent<UI_LifeMonitor>().gameObject.activeSelf)
            {
                _lifeMonitor.GetComponent<UI_LifeMonitor>().StopAudio();
            }
        }
        
        if(
            newGameState == GameStateManager.GameState.Paused ||
            newGameState == GameStateManager.GameState.Crafting ||
            newGameState == GameStateManager.GameState.Dialogue ||
            newGameState == GameStateManager.GameState.Loading
        )
        {
            _playerInput.Journal.Disable();
        }
        else _playerInput.Journal.Enable();
    }
    
    private void OnJournalExculusiveClose(InputAction.CallbackContext context)
    {
        if(IsOpen == true) GameStateManager.Instance.ResetLastState();
    }
}
