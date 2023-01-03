using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemRegister : MonoBehaviour
{
    [System.Serializable]
    public struct ItemSpritePair
    {
        public Item.ItemType type;
        public Sprite sprite;
    }

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
    [SerializeField] private List<ItemSpritePair> _itemSprites = new List<ItemSpritePair>();
    private List<Item> _items = new List<Item>()
    {
        new Item {
            itemType = Item.ItemType.Item1,
            amount = 1,
            stackable = true,
            Use = delegate(int i){ GameManager.Instance.player.GetComponent<Player>().GetInventory().RemoveItem(
                Item.ItemType.Item1,
                i
            ); }
        },
        new Item {
            itemType = Item.ItemType.Item2,
            amount = 1,
            stackable = true,
            Use = delegate(int i){ GameManager.Instance.player.GetComponent<Player>().GetInventory().RemoveItem(
                Item.ItemType.Item2,
                i
            ); }
        },
        new Item {
            itemType = Item.ItemType.Item3,
            amount = 1,
            stackable = true,
            Use = delegate(int i){ GameManager.Instance.player.GetComponent<Player>().GetInventory().RemoveItem(
                Item.ItemType.Item3,
                i
            ); }
        },
        new Item {
            itemType = Item.ItemType.Item4,
            amount = 1,
            stackable = false,
            Use = delegate(int i){ GameManager.Instance.player.GetComponent<Player>().GetInventory().RemoveItem(
                Item.ItemType.Item4,
                i
            ); }
        }
    };

    public Item GetNewItem(Item.ItemType itemType)
    {
        Item item = _items.Find(x => x.itemType == itemType);
        Item copy = new Item {
            itemType = item.itemType,
            amount = item.amount,
            stackable = item.stackable,
            Use = item.Use
        };
        return copy;
    }

    public Sprite GetItemSprite(Item.ItemType type)
    {
        return _itemSprites.Find(x => x.type == type).sprite;
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
