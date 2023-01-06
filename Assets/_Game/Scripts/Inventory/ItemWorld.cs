using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ItemWorld : MonoBehaviour, IInteractable
{
    [SerializeField] private Item.ItemType _itemType;
    [SerializeField] private int _amount;

    private LocalKeyword _OUTLINE_ON;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");

        GetComponent<SpriteRenderer>().sprite = ItemRegister.Instance.items.Find(x => x.itemType == _itemType).sprite;
    }

    ///<summary>
    ///Sets an item type and its amount.
    ///Sets correct sprite
    ///</summary>
    ///<param name="itemType"></param>
    ///<param name="amount"></param>
    public void SetItem(Item.ItemType itemType, int amount)
    {
        _itemType = itemType;
        _amount = amount;
        GetComponent<SpriteRenderer>().sprite = ItemRegister.Instance.items.Find(x => x.itemType == _itemType).sprite;
    }

    ///<summary>
    ///Returns an Item from world item
    ///</summary>
    public Item GetItem()
    {
        Item item = ItemRegister.Instance.GetNewItem(_itemType);
        item.amount = _amount;
        return item;
    }

    public void CursorClick()
    {
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.GetComponent<Player>().GetInventory().AddItem(GetItem());
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Player is not set in the GameManager singleton or is not set!");
        }
    }

    public void CursorEnter()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }
}