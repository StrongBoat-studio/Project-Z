using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput _playerInput;

    private Inventory _inventory;
    [SerializeField] private RectTransform _uiInventory;

    private void Awake()
    {
        GameManager.Instance.player = transform;

        _playerInput = new PlayerInput();
        _playerInput.Inventory.Enable();
        _playerInput.Inventory.Open.performed += OnInventoryOpen;
    }

    private void Start()
    {
        _inventory = new Inventory();
        _uiInventory.GetComponent<UI_Inventory>().SetInventory(_inventory);
        _uiInventory.GetComponent<UI_Inventory>().SetPlayer(transform);
    }

    private void Update()
    {
        
    }

    private void OnInventoryOpen(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _uiInventory.GetComponent<UI_Inventory>().ToggleInventoryPanel();
    }

    public void Heal(float healValue)
    {
        Debug.Log("Healed " + healValue + " HP");
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
}
