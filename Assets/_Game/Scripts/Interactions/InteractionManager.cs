using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    private PlayerInput _playerInput;

    private Transform _lastOver = null;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Interactions.Enable();
        _playerInput.Interactions.Interact.performed += OnClick;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.Interactions.Interact.performed -= OnClick;
    }

    ///<summary>
    ///Called when mouse is clicked. 
    ///Checks whether the clicked object has IInteractable interface assigend and calls the method OnClick()
    ///</summary>
    private void OnClick(InputAction.CallbackContext context)
    {
        if (_lastOver == null) return;
        _lastOver.GetComponent<IInteractable>()?.CursorClick();
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if(newGameState == GameStateManager.GameState.Gameplay) 
        {
            _playerInput.Interactions.Enable();
        }
        else _playerInput.Interactions.Disable();
    }

    private void Update()
    {
        //Try to get camera reference if it's not already set
        if (_cam == null)
        {
            _cam = GameObject.FindObjectOfType<Camera>();
        }
        else
        {
            ProcessInteraction();
        }
    }

    private void ProcessInteraction()
    {
        Vector2 mousePos = _playerInput.Interactions.MousePosition.ReadValue<Vector2>();
        RaycastHit2D hit = Physics2D.Raycast(_cam.ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hit == true)
        {
            //1. Mouse enters new interactable
            //2. Mouse enters new interactable without leaving the previus one
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (_lastOver == null && interactable != null)
            {
                _lastOver = hit.transform;
                _lastOver.GetComponent<IInteractable>()?.CursorEnter();
            }
            else if (_lastOver != null && interactable != null && _lastOver != hit.transform)
            {
                _lastOver.GetComponent<IInteractable>()?.CursorExit();
                _lastOver = null;

                _lastOver = hit.transform;
                _lastOver.GetComponent<IInteractable>()?.CursorEnter();
            }
        }
        else
        {
            //1. Mouse exits interactable
            if (_lastOver != null)
            {
                _lastOver.GetComponent<IInteractable>()?.CursorExit();
                _lastOver = null;
            }
        }
    }
}
