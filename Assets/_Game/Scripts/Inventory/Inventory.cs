using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

[System.Serializable]
public class Inventory
{
    private List<Item> _items;
    [SerializeField] private int _inventorySize = 10;

    public delegate void OnInventoryChangedHandler();
    public event OnInventoryChangedHandler OnInventoryChanged;

    public Inventory(int size = 10)
    {
        this._inventorySize = size;
        _items = new List<Item>();
    }

    /// <summary>
    /// Adds given Item to the inventory if there is available space
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if (_items.Count >= _inventorySize)
        {
            Debug.LogWarning("Inventory is full.");
            return;
        }

        if (_items.Find(x => x.itemType == item.itemType) == null || item.stackable == false)
        {
            _items.Add(item);
            OnInventoryChanged?.Invoke();
        }
        else
        {
            _items.Find(x => x.itemType == item.itemType).amount += item.amount;
            OnInventoryChanged?.Invoke();
        }
    }

    ///<summary>
    ///Removes give item from inventory
    ///</summary>
    ///<param name="item"></param>
    public void RemoveItem(Item.ItemType itemType, int amount)
    {
        Item itemRemove = _items.Find(x => x.itemType == itemType);

        if (itemRemove == null)
        {
            Debug.LogWarning("Cannot remove item from inventory. Item is not in the inventory.");
            return;
        }

        if (itemRemove.amount < amount)
        {
            Debug.LogWarning("Cannot remove item. Not enought of it in the inventory");
            return;
        }
        else
        {
            itemRemove.amount -= amount;

            if(itemRemove.amount <= 0)
            {
                _items.Remove(itemRemove);
            }

            OnInventoryChanged?.Invoke();
        }
    }

    /// <summary>
    /// Gets referece to inventory slots
    /// </summary>
    /// <returns>List of inventory slots</returns>
    public List<Item> GetInventoryItems()
    {
        return _items;
    }

    public int GetSize()
    {
        return _inventorySize;
    }
}