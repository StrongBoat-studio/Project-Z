using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Crafting : MonoBehaviour
{
    [SerializeField] private List<CraftingSlot> _uiItemSlots;

    private void OnEnabled()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if(GameManager.Instance.player != null)
        {
            
        }
        else
        {
            Debug.LogWarning("Player reference not found!");
        }
    }
}
