using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory _inventory;

    [SerializeField] private RectTransform _cardSlot;
    [SerializeField] private RectTransform _ammoSlot;
    [SerializeField] private RectTransform _medsSlot;
    [SerializeField] private RectTransform _flashlightSlot;

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
        if(_inventory == null)
        {
            if(GameManager.Instance.player == null) return;

            SetInventory(GameManager.Instance.player.GetComponent<Player>().GetInventory());
        }
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.OnInventoryChanged += OnInventoryChanged;
        OnInventoryChanged();
    }

    private void OnInventoryChanged()
    {
        foreach(var slot in _inventory.GetInventorySlots())
        {
            if(slot.HasItem(ItemRegister.Instance.card))
            {
                _cardSlot.GetComponentInChildren<Image>().sprite = slot.GetData().item.GetItemSprite();
            }

            if(slot.HasItem(ItemRegister.Instance.ammunition))
            {
                _ammoSlot.GetComponentInChildren<Image>().sprite = slot.GetData().item.GetItemSprite();
                _ammoSlot.GetComponentInChildren<TextMeshProUGUI>().text = slot.GetData().quantity.ToString();
            }

            if(slot.HasItem(ItemRegister.Instance.medicaments))
            {
                _medsSlot.GetComponentInChildren<Image>().sprite = slot.GetData().item.GetItemSprite();
                _medsSlot.GetComponentInChildren<TextMeshProUGUI>().text = slot.GetData().quantity.ToString();
            }

            if(slot.HasItem(ItemRegister.Instance.flashlight))
            {
                _flashlightSlot.GetComponentInChildren<Image>().sprite = slot.GetData().item.GetItemSprite();
                _flashlightSlot.GetComponentInChildren<TextMeshProUGUI>().text = slot.GetData().quantity.ToString();
            }
        }
        
    }
}
