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
        private Item _item;
        private int _quantity;

        public InventorySlot(Item item, int quantity)
        {
            this._item = item;
            this._quantity = quantity;
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
            _item = null;
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
    }

    private List<InventorySlot> _inventorySlots = new List<InventorySlot>();
    private int _inventorySize = 10;

    public Inventory(int size = 10)
    {
        this._inventorySize = size;

        //Populate inventory
        for (int i = 0; i < _inventorySize; i++)
        {
            _inventorySlots.Add(new InventorySlot(null, 0));
        }
    }

    /// <summary>
    /// Adds given Item to the inventory if there is available space
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item, int quantity)
    {
        //Inventory is full 
        if (GetEmptyInventorySlot() == null) return;

        InventorySlot slot = GetSlotWithItem(item);

        if (slot == null)
        {
            //Item is not in the inventory
            var returnedItem = GetEmptyInventorySlot().AddItem(item, quantity);
            if (returnedItem.item != null)
            {
                Debug.Log("Function 'GetEmptyInventorySlot().AddItem()' from Inventory.AddItem() returned an item!");
            }
        }
        else
        {
            //Item is already in the inventory, increase quantity
            slot.AddQuantity(quantity);
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

        return slot.RemoveQuantity(quantity);
    }

    /// <summary>
    /// Gets referece to slot if given item already exists in inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns>InventorySlot</returns>
    private InventorySlot GetSlotWithItem(Item item)
    {
        InventorySlot slot = null;

        foreach (InventorySlot s in _inventorySlots)
        {
            if (s.GetData().item == item) slot = s;
        }

        return slot;
    }

    /// <summary>
    /// Gets first empty inventory slot
    /// </summary>
    /// <returns>InventorySlot</returns>
    private InventorySlot GetEmptyInventorySlot()
    {
        foreach (InventorySlot s in _inventorySlots)
        {
            if (s.GetData().item == null) return s;
        }

        return null;
    }
}