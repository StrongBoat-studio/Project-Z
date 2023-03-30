using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    private PlayerInput _playerInput;

    private Transform _lastOver = null;

    public Transform LastOver { get { return _lastOver; } }

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
        RaycastHit2D[] hits = Physics2D.RaycastAll(_cam.ScreenToWorldPoint(mousePos), Vector2.zero);
        RaycastHit2D hitInteractable = hits.FirstOrDefault(x => x.collider.GetComponent<IInteractable>() != null);

        if (hits.Length > 0 && hitInteractable)
        {
            //1. Mouse enters new interactable
            //2. Mouse enters new interactable without leaving the previus one
            IInteractable interactable = hitInteractable.collider.GetComponent<IInteractable>();

            if (_lastOver == null && interactable != null)
            {
                _lastOver = hitInteractable.transform;
                _lastOver.GetComponent<IInteractable>()?.CursorEnter();
            }
            else if (_lastOver != null && interactable != null && _lastOver != hitInteractable.transform)
            {
                _lastOver.GetComponent<IInteractable>()?.CursorExit();
                _lastOver = null;

                _lastOver = hitInteractable.transform;
                _lastOver.GetComponent<IInteractable>()?.CursorEnter();
            }
            else if(_lastOver != null && interactable == null)
            {
                _lastOver?.GetComponent<IInteractable>()?.CursorExit();
                _lastOver = null;
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
