using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Item1,
        Item2,
        Item3,
        Item4
    }

    public ItemType itemType;
    public int amount;
    public bool stackable;
    public Action<int> Use;
}
