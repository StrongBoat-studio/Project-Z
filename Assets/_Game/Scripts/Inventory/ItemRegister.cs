using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemRegister : MonoBehaviour
{
    public static ItemRegister Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject worldItemPrefab;
    public List<Item> items = new List<Item>();

    public Item GetNewItem(Item.ItemType itemType)
    {
        Item item = items.Find(x => x.itemType == itemType);
        Item copy = new Item {
            itemType = item.itemType,
            amount = item.amount,
            sprite = item.sprite,
            stackable = item.stackable,
            Use = item.Use
        };
        return copy;
    }

    /// <summary>
    /// Create item gameobejct and place it on given position
    /// </summary>
    /// <param name="item">Item to create</param>
    /// <param name="position">Position on which the item will be created</param>
    public void CreateWorldItem(Item.ItemType itemType, int amount, Vector2 position)
    {
        Transform worldItem = Instantiate(worldItemPrefab, position, Quaternion.identity).transform;
        worldItem.GetComponent<ItemWorld>().SetItem(itemType, amount);
    }
}
