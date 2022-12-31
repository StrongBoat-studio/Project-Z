using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ItemWorld : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _item;
    [Min(1f)][SerializeField] private int _quantity = 1;

    private LocalKeyword _OUTLINE_ON;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
        if (!_item.HasValidData()) return;

        GetComponent<SpriteRenderer>().sprite = _item.GetItemSprite();
    }

    ///<summary>
    ///Sets an item and its quantity. (quantity >= 1)
    ///Sets correct sprite
    ///Adjusts BoxCollider2D's bounds to match sprite
    ///</summary>
    ///<param name="item"></param>
    ///<param name="quantity"></param>
    public void SetItem(Item item, int quantity = 1)
    {
        _item = item;
        _quantity = (quantity <= 0) ? 1 : quantity; //Make sure to set quantity >= 1
        GetComponent<SpriteRenderer>().sprite = _item.GetItemSprite();
        GetComponent<BoxCollider2D>().size = _item.GetItemSprite().bounds.size;
    }

    public void CursorClick()
    {
        Debug.Log("Clicked " + _item.GetName());
        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.GetComponent<Player>().AddItem(_item, _quantity);
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