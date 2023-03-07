using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Crafting : MonoBehaviour
{
    [Tooltip("Crafting script")]
    [SerializeField] private Crafting _crafting;
    [Tooltip("Crafting item slots")]
    [SerializeField] private List<CraftingSlot> _uiItemSlots;
    public CraftingSlot[] UIItemSlots { get => _uiItemSlots.ToArray(); }
    public CraftingSlot EmptySlot { get => _uiItemSlots.Find(x => x.ItemType == Item.ItemType.None); }

    [Tooltip("Crafting progress bar")]
    [SerializeField] private Image _craftingProgress;
    [Tooltip("Smoothness of progress bar fill")]
    [SerializeField] private float _fillStepTime = .2f;

    private void Update()
    {
        //Update fill
        float currFill = (float)_crafting.GetRecipeProgressItemCount() / _crafting.GetRecipeItemCount();
        _craftingProgress.DOFillAmount(currFill, _fillStepTime);
    }

    public void SetSlotItem(CraftingSlot slot, Item.ItemType type)
    {
        slot.SetItem(type);
    }

    public void Reset()
    {
        foreach(var slot in _uiItemSlots)
        {
            slot.Reset();
        }
    }
}
