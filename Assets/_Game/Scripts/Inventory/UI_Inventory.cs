using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory _inventory;
    private Transform _player;

    [Header("Hotbar")]
    [SerializeField] private RectTransform _hotbar;

    [Header("Panel")]
    [SerializeField] private RectTransform _inventorySlotPrefab;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private RectTransform _panelSlotContainer;

    private void Start()
    {
        
    }

    private void Destroy()
    {
        if(_inventory == null) return;
        _inventory.OnInventoryChanged -= OnInventoryChanged;
    }

    private void Update()
    {

    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.OnInventoryChanged += OnInventoryChanged;
        OnInventoryChanged();
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    private void OnInventoryChanged()
    {
        UpdatePanel();
        UpdateHotbar();
    }

    private void UpdatePanel()
    {
        for(int i = 0; i < _panelSlotContainer.childCount; i++)
        {
            Destroy(_panelSlotContainer.GetChild(i).gameObject);
        }

        foreach(var item in _inventory.GetInventoryItems())
        {
            RectTransform slot = Instantiate(_inventorySlotPrefab, Vector3.zero, Quaternion.identity, _panelSlotContainer);
            slot.GetComponent<UI_InventorySlot>().SetItem(item);
        }
    }

    private void UpdateHotbar()
    {
        
    }

    public void ToggleInventoryPanel()
    {
        _panel.gameObject.SetActive(!_panel.gameObject.activeSelf);
    }
}
