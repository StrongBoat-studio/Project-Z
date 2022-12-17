using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Interactions.Enable();
        _playerInput.Interactions.Interact.performed += OnClick;
    }

    ///<summary>
    ///Called when mouse is clicked. 
    ///Checks whether the clicked object has IInteractable interface assigend and calls the method OnClick()
    ///</summary>
    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = _playerInput.Interactions.MousePosition.ReadValue<Vector2>();
        RaycastHit2D hit = Physics2D.Raycast(_cam.ScreenToWorldPoint(mousePos), Vector2.zero);

        if(hit == false) return;
        
        Debug.Log("Clicked something");
        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        interactable?.OnClicked();
    }
}
