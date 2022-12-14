using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] private ItemScriptableObject _itemData;
    [Min(1f)][SerializeField] private int _quantity;

    public Sprite GetItemSprite()
    {
        return _itemData.itemSprite;
    }
}
