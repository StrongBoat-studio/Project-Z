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
        Knife = 1,
        Gun = 2,
        Ammo = 3,
        Drool1 = 4,
        Drool2 = 5,
        GunPowder = 6,
        Herb = 7,
        Note = 8,
        Vile1 = 9,
        Vile2 = 10,
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
