using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    //Default to an empty item 
    [SerializeField] private ItemScriptableObject _itemData = ItemRegister.Instance.emptyItem;

    public Item(ItemScriptableObject itemData)
    {
        this._itemData = itemData;
    }

    private ItemScriptableObject GetItemScriptableObject()
    {
        return _itemData;
    }

    public Sprite GetItemSprite()
    {
        return _itemData.itemSprite;
    }

    public string GetName() 
    {
        return _itemData.itemName;
    }

    public bool CompareItems(Item item)
    {
        return item.GetItemScriptableObject() == _itemData;
    }

    public bool IsEmpty()
    {
        return _itemData == ItemRegister.Instance.emptyItem;
    }
}
