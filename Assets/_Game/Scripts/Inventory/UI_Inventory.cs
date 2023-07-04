using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    [Header("Tooltip box")]
    [SerializeField] private RectTransform _tooltipBox;
    [SerializeField] private TextMeshProUGUI _tooltipName;
    [SerializeField] private TextMeshProUGUI _tooltipDescription;
    private bool _tweening = false;

    private void OnDestroy()
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            UpdatePanel();
        }
    }

    public void UpdatePanel()
    {
        HideToolTip();
        Debug.Log("UpdatePanel!");
        for (int i = 0; i < _panelSlotContainer.childCount; i++)
        {
            Destroy(_panelSlotContainer.GetChild(i).gameObject);
        }

        for (int x = 0; x < _maxColumns; x++)
        {
            for (int y = 0; y < Mathf.Ceil(_inventory.GetSize() / _maxColumns); y++)
            {
                if (y * _maxColumns + x < _inventory.GetInventoryItems().Count)
                {
                    RectTransform slot = Instantiate(_inventorySlotPrefab, Vector3.zero, Quaternion.identity, _panelSlotContainer);
                    slot.GetComponent<UI_InventorySlot>().SetItem(_inventory.GetInventoryItems()[y * _maxColumns + x], this, new Vector2(x, y));
                    slot.anchoredPosition = new Vector3(
                        _offset.x + _paddingLeft + _cellWidth / 2 + x * (_cellWidth + _cellPaddingRight),
                        _offset.y + (-_paddingTop) - _cellHeight / 2 - y * (_cellHeight + _cellPaddingBottom)
                    );
                }
            }
        }
    }

    public void ShowTooltip(RectTransform itemSlot, string itemName, TextAsset itemDescription, Vector2 posIndex)
    {
        _tooltipBox.SetParent(itemSlot);
        //Show tooltip on the right for slots on the left half and on the left for slots on the right half
        _tooltipBox.anchoredPosition = new Vector2(
            (posIndex.x >= _inventory.GetSize() / 4) ? -_tooltipBox.rect.width : 0f,
            _tooltipBox.rect.height + itemSlot.rect.height / 2
        );
        _tooltipName.text = itemName;
        _tooltipDescription.text = itemDescription.text;
        _tooltipBox.SetParent(_panel);
        _tooltipBox.GetComponent<CanvasGroup>().DOFade(1f, .2f);
    }

    public void HideToolTip()
    {
        _tooltipBox.GetComponent<CanvasGroup>().DOFade(0f, .2f);
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
