using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

[System.Serializable]
public class Inventory
{
    private List<Item> _items;
    public List<Item> Items { get => _items; }
    [SerializeField] private int _inventorySize = 12;

    public bool IsFull { get => _items.Count >= _inventorySize; }

    public delegate void OnInventoryChangedHandler();
    public event OnInventoryChangedHandler OnInventoryChanged;

    public Inventory(int size = 12)
    {
        this._inventorySize = size;
        _items = new List<Item>();
    }

    /// <summary>
    /// Adds given Item to the inventory if there is available space
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(Item item, bool silent = false)
    {
        if (
            _items.Count >= _inventorySize &&
            (
                _items.Find(x => x.itemType == item.itemType) == null ||
                (_items.Find(x => x.itemType == item.itemType) != null && !item.stackable)
            )
        )
        {
            Debug.LogWarning("Inventory is full.");
            return false;
        }

        if (_items.Find(x => x.itemType == item.itemType) == null || item.stackable == false)
        {
            //Item dialogue note on add only for new items
            if (DialogueManager.Instance != null && _items.Find(x => x.itemType == item.itemType) == null && silent == false)
            {
                DialogueManager.Instance.EnqueueStory(item.collectDialogue);
            }

            _items.Add(item);
            OnInventoryChanged?.Invoke();

            //Update Quest
            if(QuestLineManager.Instance != null)
            {
                QuestLineManager.Instance.CheckQuestItems(_items);
            }
        }
        else
        {
            _items.Find(x => x.itemType == item.itemType).amount += item.amount;
            OnInventoryChanged?.Invoke();

            //Update Quest
            if(QuestLineManager.Instance != null)
            {
                QuestLineManager.Instance.CheckQuestItems(_items);
            }
        }
        return true;
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

            if (itemRemove.amount <= 0)
            {
                _items.Remove(itemRemove);
            }

            OnInventoryChanged?.Invoke();

            //Update Quest
            if(QuestLineManager.Instance != null)
            {
                QuestLineManager.Instance.CheckQuestItems(_items);
            }
        }
    }

    /// <summary>
    /// Checks whether removing this item will result in
    /// emptying any inventory slots
    /// </summary>
    /// <returns>True if slot will be emptied</returns>
    public bool WillRemoveItemClearSlot(Item.ItemType type, int amount)
    {
        if(_items.Find(x => x.itemType == type).amount == amount) return true;
        else return false;
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

    public void LoadSave(List<GameData.InventoryItemState> inventoryItems)
    {
        if(inventoryItems.Count <= 0) return;

        foreach(var item in inventoryItems)
        {
            Item i = ItemRegister.Instance.GetNewItem((Item.ItemType)item.type);
            i.amount = item.amount;
            AddItem(i, true);
        }
    }
}