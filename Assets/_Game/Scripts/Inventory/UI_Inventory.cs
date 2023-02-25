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

    [Header("Panel")]
    [SerializeField] private RectTransform _inventorySlotPrefab;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private RectTransform _panelSlotContainer;

    [Header("Panel grid")]
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _paddingLeft;
    [SerializeField] private float _paddingTop;
    [SerializeField] private int _maxColumns;
    [SerializeField] private float _cellWidth;
    [SerializeField] private float _cellHeight;
    [SerializeField] private float _cellPaddingRight;
    [SerializeField] private float _cellPaddingBottom;

    private void Destroy()  
    {
        if (_inventory == null) return;
        _inventory.OnInventoryChanged -= OnInventoryChanged;
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
    }

    private void UpdatePanel()
    {
        Debug.Log("UpdatePanel!");
        for (int i = 0; i < _panelSlotContainer.childCount; i++)
        {
            Destroy(_panelSlotContainer.GetChild(i).gameObject);
        }

        for (int x = 0; x < _maxColumns; x++)
        {
            for (int y = 0; y < Mathf.Ceil(_inventory.GetSize() / _maxColumns); y++)
            {
                if (y * 5 + x < _inventory.GetInventoryItems().Count)
                {
                    RectTransform slot = Instantiate(_inventorySlotPrefab, Vector3.zero, Quaternion.identity, _panelSlotContainer);
                    slot.GetComponent<UI_InventorySlot>().SetItem(_inventory.GetInventoryItems()[y * 5 + x]);
                    slot.anchoredPosition = new Vector3(
                        _offset.x + _paddingLeft + _cellWidth / 2 + x * (_cellWidth + _cellPaddingRight),
                        _offset.y + (-_paddingTop) - _cellHeight / 2 - y * (_cellHeight + _cellPaddingBottom)
                    );
                }
            }
        }
    }

    public void ToggleInventoryPanel(bool state)
    {
        _panel.gameObject.SetActive(state);
    }

    public bool IsOpen()
    {
        return _panel.gameObject.activeSelf;
    }
}
