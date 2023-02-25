using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Item _item;
    private Vector2 _dragStartPos;

    [SerializeField] private RectTransform _itemSprite;

    ///<summary>
    ///Set item's  sprite and amount
    ///</summary>
    ///<param name="item"></param>
    public void SetItem(Item item)
    {
        _item = item;
        transform.Find("Item").GetComponent<Image>().sprite = item.sprite;
        transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = item.amount.ToString();
    }

    public Item GetItem()
    {
        return _item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.dragging) return;
            _item.Use?.Invoke(_item, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Move to top to render above all items
        transform.SetAsLastSibling();

        //Save start drag pos to snap back when ending the drag
        _dragStartPos = _itemSprite.anchoredPosition;

        //Center the dragged object to center of mouse
        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _itemSprite, eventData.position, null, out delta
        );
        _itemSprite.anchoredPosition += delta;

        //Disable raycast blocking and set visualas
        _itemSprite.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _itemSprite.GetComponent<CanvasGroup>().alpha = 0.75f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _itemSprite.GetComponent<RectTransform>().anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Reset pos, enable raycast blocking and set visuals
        _itemSprite.anchoredPosition = _dragStartPos;
        _itemSprite.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _itemSprite.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemCraft.Instance.TryCraft(
            this, 
            eventData.pointerDrag.GetComponent<UI_InventorySlot>()
        );
    }
}
