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
    private Item.ItemType _recipeOutput = Item.ItemType.Gun;
    private List<Item.ItemType> _recipeProgress;
    private PlayerInput _playerInput;
    private LocalKeyword _OUTLINE_ON;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(_craftingSprite.GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");

        _playerInput = new PlayerInput();
        _playerInput.Crafting.Enable();
        _playerInput.Crafting.CloseExclusive.performed += OnCloseExclusivePerformed;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        _recipeProgress = new List<Item.ItemType>();
    }

    private void Destroy()
    {
        _playerInput.Crafting.CloseExclusive.performed -= OnCloseExclusivePerformed;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void AddIngridient(Item.ItemType itemType)
    {
        _recipeProgress.Add(itemType);
        CheckCrafting();
    }

    private void CheckCrafting()
    {
        if (_recipeProgress.Count != _recipe.Count)
        {
            if (_recipeProgress.Count > _recipe.Count)
            {
                //Debug
                Debug.LogAssertion($"Player added more ingridients ({_recipeProgress.Count}) than recipe is set to handle! ({_recipe.Count})");
            }
            return;
        }

        for (int i = 0; i < _recipe.Count; i++)
        {
            if (_recipe[i] != _recipeProgress[i])
            {
                //Debug
                Debug.LogAssertion("Recipe entered by player is different then desired recipe yet it still passed tests");
                return;
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            Item item = ItemRegister.Instance.GetNewItem(_recipeOutput);
            item.amount = 1;
            GameManager.Instance.player.GetComponent<Player>().GetInventory().AddItem(item);
        }
    }

    public Item.ItemType GetNextCraftingStep()
    {
        if (_recipeProgress.Count == _recipe.Count) return Item.ItemType.None;

        return _recipe[_recipeProgress.Count];
    }

    public int GetRecipeItemCount()
    {
        return _recipe.Count;
    }

    public int GetRecipeProgressItemCount()
    {
        return _recipeProgress.Count;
    }

    private void OnCloseExclusivePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.Crafting)
        {
            GameStateManager.Instance.ResetLastState();
        }
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        //Crafting window should be only closed when pause menu is opened
        //Crafting window has priority over other UI elements
        if(newGameState == GameStateManager.GameState.Crafting)
            _uiCrafting.gameObject.SetActive(true);
        else if(
            GameStateManager.Instance.HasStateInHistory(GameStateManager.GameState.Crafting) &&
            newGameState != GameStateManager.GameState.Paused
        )
        {
            _uiCrafting.gameObject.SetActive(true);
        }
        else
        {
            _uiCrafting.gameObject.SetActive(false);
        }

        if (newGameState != GameStateManager.GameState.Crafting)
            _playerInput.Crafting.Disable();
        else
            _playerInput.Crafting.Enable();
    }

    public void CursorClick()
    {
        if (GameStateManager.Instance.GetCurrentState() != GameStateManager.GameState.Crafting)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Crafting);
            _uiCrafting.gameObject.SetActive(true);
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
