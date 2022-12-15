using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] private ItemScriptableObject _itemData;

    public Item(ItemScriptableObject itemData)
    {
        this._itemData = itemData;
    }

    public Sprite GetItemSprite()
    {
        return _itemData.itemSprite;
    }

    public ItemScriptableObject GetItemScriptableObject()
    {
        return _itemData;
    }
}
