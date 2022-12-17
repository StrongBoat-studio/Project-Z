using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ItemWorld : MonoBehaviour
{
    [SerializeField] private Item _item;
    [Min(1f)][SerializeField] private int _quantity = 1;

    private void Awake()
    {
        if(_item.IsEmpty() == true) return;

        GetComponent<SpriteRenderer>().sprite = _item.GetItemSprite();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void SetItem(Item item, int quantity = 1)
    {
        _item = item;
        _quantity = quantity;
        GetComponent<SpriteRenderer>().sprite = _item.GetItemSprite();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}