using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Item _item;
    private bool _dragging = false;
    private Vector2 _dragStartPos;

    [SerializeField] private RectTransform _itemSprite;
    [SerializeField] private RectTransform _itemAmount;
    private Transform _parent;

    private void Awake()
    {
        _parent = transform.parent;
    }

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
        if(_dragging) return;
            _item.Use?.Invoke(_item, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _dragging = true;
        _dragStartPos = _itemSprite.anchoredPosition;
        _itemSprite.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _itemSprite.GetComponent<CanvasGroup>().alpha = 0.75f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _itemSprite.GetComponent<RectTransform>().anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragging = false;
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
