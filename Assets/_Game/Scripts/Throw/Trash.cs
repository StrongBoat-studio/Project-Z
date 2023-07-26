using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public class Trash
{
    public enum TrashType
    {
        Trash1 = 0,
        Trash2 = 1,
        Trash3 = 2,
        Trash4 = 3,
        Trash5 = 4,
        Trash6 = 5,
    }

    public TrashType trashType;
    public Sprite sprite;
    public int damage;
}
