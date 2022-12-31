using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        private int _index;
        private Item _item;
        private int _quantity;

        public InventorySlot(int index)
        {
            this._index = index;
            _item = new Item(null);
            _quantity = 0;
        }

        /// <summary>
        /// Get item and it's count
        /// </summary>
        /// <returns>Tuple of Item and Int</returns>
        public (Item item, int quantity) GetData()
        {
            return (_item, _quantity);
        }

        /// <summary>
        /// Adds Item to the slot
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <returns>Previus Item and its quantity</returns>
        public (Item item, int quantity) AddItem(Item item, int quantity)
        {
            var toRemove = GetData();
            _item = item;
            _quantity = quantity;
            return toRemove;
        }

        /// <summary>
        /// Removes an Item from slot
        /// </summary>
        /// <returns>Item from the slot and its quantity</returns>
        private void RemoveItem()
        {
            _item = new Item(null);
            _quantity = 0;
        }

        /// <summary>
        /// Adds to the item quantity
        /// </summary>
        /// <param name="quantity"></param>
        public bool AddQuantity(int quantity)
        {
            _quantity += quantity;
            return true;
        }

        /// <summary>
        /// Subtracts from item quantity is given quantity can be subtracted
        /// </summary>
        /// <param name="quantity"></param>
        public bool RemoveQuantity(int quantity)
        {
            if (_quantity < quantity)
            {
                //Don't remove, too little qunatity is in the inventory
                Debug.Log("Canno remove given quantity(" + quantity + ") of this Item! Only " + _quantity + " in the inventory slot!");
                return false;
            }
            else if (_quantity == quantity)
            {
                //Remove whole item ftom this slot, quantoty to remove equals the quantity in the inventory
                RemoveItem();
                return true;
            }
            else
            {
                //Subtract the given quantity from inventory slot
                _quantity -= quantity;
                return true;
            }
        }

        /// <summary>
        /// Checks if the slot is empty
        /// </summary>
        /// <returns>True if empty</returns>
        public bool IsEmpty()
        {
            //return _item.IsEmpty();
            return !_item.HasValidData();
        }

        /// <summary>
        /// Checks if slot is has a specific item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if slot contains given item</returns>
        public bool HasItem(Item item)
        {
            if (!_item.HasValidData()) return false;
            if (_item.CompareItems(item) == true) return true;

            return false;
        }
    }

    private List<InventorySlot> _inventorySlots = new List<InventorySlot>();
    private int _inventorySize = 10;

    public delegate void OnInventoryChangedHandler();
    public event OnInventoryChangedHandler OnInventoryChanged;

    public Inventory(int size = 10)
    {
        this._inventorySize = size;

        //Populate inventory
        for (int i = 0; i < _inventorySize; i++)
        {
            _inventorySlots.Add(new InventorySlot(i));
        }
    }

    /// <summary>
    /// Adds given Item to the inventory if there is available space
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item, int quantity)
    {
        InventorySlot emptySlot = GetEmptyInventorySlot();
        InventorySlot itemInSlot = GetSlotWithItem(item);

        //Inventory is full 
        if (emptySlot == null && itemInSlot == null)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        if (itemInSlot == null)
        {
            //Item is not in the inventory, add new
            var returnedItem = emptySlot.AddItem(item, quantity);
            OnInventoryChanged?.Invoke();
            if(returnedItem.item.HasValidData())
            {
                Debug.Log("Function 'GetEmptyInventorySlot().AddItem()' from Inventory.AddItem() returned an item but it shouldn't!");
            }
        }
        else
        {
            //Item is already in the inventory, increase quantity
            itemInSlot.AddQuantity(quantity);
        }
    }

    public bool RemoveItem(Item item, int quantity)
    {
        InventorySlot slot = GetSlotWithItem(item);

        //Item is not in the inventory
        if (slot == null)
        {
            Debug.Log("Couldn't find a slot with this item!");
            return false;
        }

        OnInventoryChanged?.Invoke();
        return slot.RemoveQuantity(quantity);
    }

    /// <summary>
    /// Gets referece to inventory slots
    /// </summary>
    /// <returns>List of inventory slots</returns>
    public List<InventorySlot> GetInventorySlots()
    {
        return _inventorySlots;
    }

    /// <summary>
    /// Gets referece to slot if given item already exists in inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns>InventorySlot</returns>
    private InventorySlot GetSlotWithItem(Item item)
    {
        return _inventorySlots.Find(s => s.HasItem(item));
    }

    /// <summary>
    /// Gets first empty inventory slot
    /// </summary>
    /// <returns>InventorySlot</returns>
    private InventorySlot GetEmptyInventorySlot()
    {
        return _inventorySlots.Find(s => s.IsEmpty() == true);
    }
}