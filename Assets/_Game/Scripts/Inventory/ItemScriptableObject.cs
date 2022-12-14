using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item", fileName = "Item Data")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
}