using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Crafting : MonoBehaviour, IInteractable
{
    [SerializeField] private UI_Crafting _uiCrafting;
    [SerializeField] private Transform _craftingSprite;

    //Hard coded recipe for the ONE and ONLY crafting in the game!
    //Will be used it in this crafting table only! YAAAAAAAAAAAAAAAY!!
    private List<Item.ItemType> _recipe = new List<Item.ItemType>(){
        Item.ItemType.Letter, 
        Item.ItemType.CD, 
        Item.ItemType.Key, 
        Item.ItemType.PC, 
        Item.ItemType.Potion
    };
    private List<Item.ItemType> _currentEnteredRecipe;
    private PlayerInput _playerInput;
    private LocalKeyword _OUTLINE_ON;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(_craftingSprite.GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
        Debug.Log(_OUTLINE_ON);

        _playerInput = new PlayerInput();
        _playerInput.Crafting.Enable();
        _playerInput.Crafting.CloseExclusive.performed += OnCloseExclusivePerformed;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        _currentEnteredRecipe = new List<Item.ItemType>();
    }

    private void Destroy()
    {
        _playerInput.Crafting.CloseExclusive.performed -= OnCloseExclusivePerformed;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged; 
    }

    private void CheckCrafting()
    {
        
    }

    private void OnCloseExclusivePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.Crafting)
        {
            GameStateManager.Instance.ResetLastState();
        }
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _uiCrafting.gameObject.SetActive(newGameState == GameStateManager.GameState.Crafting);
        
        if(newGameState != GameStateManager.GameState.Crafting)
            _playerInput.Crafting.Disable();
        else
            _playerInput.Crafting.Enable();
    }

    public void CursorClick()
    {
        if(GameStateManager.Instance.GetCurrentState() != GameStateManager.GameState.Crafting)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Crafting);
        }
    }

    public void CursorEnter()
    {
        _craftingSprite.GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
    }

    public void CursorExit()
    {
        _craftingSprite.GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    } 
}
