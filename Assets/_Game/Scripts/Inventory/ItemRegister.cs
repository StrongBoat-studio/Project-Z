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

    public GameObject worldItemPrefab;
    public Item emptyItem;
    public Item testItemWhite;
    public Item testItemLightGrey;
    public Item testItemGrey;
    public Item testItemBack;
    public Item card;
    public Item ammunition;
    public Item medicaments;
    public Item flashlight;

    /// <summary>
    /// Create item gameobejct and place it on given position
    /// </summary>
    /// <param name="item">Item to create</param>
    /// <param name="position">Position on which the item will be created</param>
    /// <param name="quantity">Amount of item i stack</param>
    public void CreateWorldItem(Item item, Vector2 position, int quantity = 1)
    {
        Transform worldItem = Instantiate(worldItemPrefab, position, Quaternion.identity).transform;
        worldItem.GetComponent<ItemWorld>().SetItem(item, quantity);
    }
}
