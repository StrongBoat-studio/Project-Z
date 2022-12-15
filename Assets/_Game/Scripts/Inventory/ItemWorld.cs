using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemWorld : MonoBehaviour
{
    [SerializeField] private Item _item;
    [Min(1f)] [SerializeField] private int _quantity;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = _item.GetItemSprite();
    }
}