using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crafting : MonoBehaviour
{
    [SerializeField] private List<CraftingSlot> _uiItemSlots;
    [SerializeField] private Image _craftingProgress;
    [SerializeField] private Crafting _crafting;
    [SerializeField] private float _fillSmoothness = 0.01f;

    private void Update()
    {
        float prevFill = _craftingProgress.fillAmount;
        float currFill = (float)_crafting.GetRecipeProgressItemCount() / _crafting.GetRecipeItemCount();
        if (currFill > prevFill) prevFill = Mathf.Min(prevFill + _fillSmoothness, currFill);
        else if (currFill < prevFill) prevFill = Mathf.Max(prevFill - _fillSmoothness, currFill);
        _craftingProgress.fillAmount = prevFill;
    }
}
