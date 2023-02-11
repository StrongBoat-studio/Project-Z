using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;
    private int _hp = 100;

    private Inventory _inventory;
    [SerializeField] private RectTransform _uiInventory;

    [SerializeField] DialogueController _lowHP;

    private void Awake()
    {
        GameManager.Instance.player = transform;

        _playerInput = new PlayerInput();
        _playerInput.Inventory.Enable();
        _playerInput.Inventory.Open.performed += OnInventoryOpen;
        _playerInput.Inventory.CloseExclusive.performed += OnInventoryCloseExclusive;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        _inventory = new Inventory();
        _uiInventory.GetComponent<UI_Inventory>().SetInventory(_inventory);
        _uiInventory.GetComponent<UI_Inventory>().SetPlayer(transform);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            TakeDamage(20);
        }
    }

    private void Destroy()
    {
        _playerInput.Inventory.Open.performed -= OnInventoryOpen;
        _playerInput.Inventory.CloseExclusive.performed -= OnInventoryCloseExclusive;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        _uiInventory.GetComponent<UI_Inventory>().ToggleInventoryPanel(newGameState == GameStateManager.GameState.Inventory);
    }

    private void OnInventoryOpen(InputAction.CallbackContext context)
    {
        if(_uiInventory.GetComponent<UI_Inventory>().IsOpen()) GameStateManager.Instance.ResetLastState();
        else GameStateManager.Instance.SetState(GameStateManager.GameState.Inventory);
    }

    private void OnInventoryCloseExclusive(InputAction.CallbackContext context)
    {
        if(_uiInventory.GetComponent<UI_Inventory>().IsOpen()) GameStateManager.Instance.ResetLastState();
    }

    private void TakeDamage(int dmg)
    {
        _hp -= dmg;

        if(_hp <= 20)
        {
            _lowHP.Play();
        }
        else if(_hp <= 0)
        {
            Debug.Log("Dead");
        }
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
}
