using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_InventorySlot : MonoBehaviour, IPointerClickHandler
{
    private Item _item;

    public void SetItem(Item item)
    {
        _item = item;
        transform.Find("Item").GetComponent<Image>().sprite = ItemRegister.Instance.GetItemSprite(item.itemType);
        transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = item.amount.ToString();
    }

    public Item GetItem()
    {
        return _item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _item.Use(1);
    }
}
