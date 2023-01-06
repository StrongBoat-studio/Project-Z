using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse : MonoBehaviour
{
    public void ConsumeItem(Item item, int amount)
    {
        GameManager.Instance.player.GetComponent<Player>().GetInventory().RemoveItem(
            item.itemType,
            amount
        );
    }
}
