using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        CD,
        Gun,
        Key,
        Letter,
        PC,
        Potion
    }

    public ItemType itemType;
    [HideInInspector] public int amount = 1;
    public Sprite sprite;
    public bool stackable;
    public UnityEvent<Item, int> Use;
}
