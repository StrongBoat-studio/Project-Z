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
        None = 0,
        CD = 1,
        Gun = 2,
        Key = 3,
        Letter = 4,
        PC = 5,
        Potion = 6
    }

    public ItemType itemType;
    [HideInInspector] public int amount = 1;
    public Sprite sprite;
    public bool stackable;
    public UnityEvent<Item, int> Use;
    public TextAsset collectDialogue;
    public TextAsset itemDecription;
    public string itemName;
}
