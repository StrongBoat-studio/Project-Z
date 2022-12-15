using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public ItemScriptableObject testItem;

    /// <summary>
    /// Create Item from itemData
    /// </summary>
    /// <returns>Item</returns>
    public Item CreateItem(ItemScriptableObject itemData)
    {
        return new Item(itemData);
    }
}
