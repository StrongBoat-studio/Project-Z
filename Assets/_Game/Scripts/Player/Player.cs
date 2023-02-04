using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnInventoryOpen(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _uiInventory.GetComponent<UI_Inventory>().ToggleInventoryPanel();
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
