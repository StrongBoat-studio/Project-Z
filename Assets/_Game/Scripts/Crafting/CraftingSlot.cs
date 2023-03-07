using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item slot")]
    [Tooltip("Item in the slot")]
    [SerializeField] private Item.ItemType _itemType = Item.ItemType.None;
    public Item.ItemType ItemType { get => _itemType; }
    [Tooltip("Item image component")]
    [SerializeField] private Image _itemImage;
    [Space]
    [Header("Slide animation")]
    [Tooltip("Selected item Y value change (slide upwards, in pixels)")]
    [SerializeField] private int _slideUp = 40;
    [Tooltip("Length of the slide animation")]
    [SerializeField] private float _slideUpTime = .2f;
    [Space]
    [Header("Shake animation")]
    [Tooltip("Shake duration")]
    [SerializeField] private float _shakeDuration = .4f;
    [Tooltip("Shake direction and strength")]
    [SerializeField] private Vector2 _shakeDirection = Vector3.right * 8;
    [Space]
    [Tooltip("QTE duration")]
    [SerializeField] private float _qteDuration = 5f;
    [Space]
    [Header("Crafting script")]
    [Tooltip("Crafting script")]
    [SerializeField] private Crafting _crafting;

    private bool _selected = false;
    private bool _craftActive = false;
    private QTEManager.Caller _qteCaller;
    private CanvasGroup _canvasGroup;
    private bool _interactable = true;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
        _canvasGroup = GetComponentInChildren<CanvasGroup>();

        SetItem(_itemType);
    }

    // Update is called once per frame
    void Update()
    {
        if (_craftActive == false) return;
        _qteCaller = QTEManager.Instance.QTEAction(QTEManager.Caller.Crafting, 1);

        if (_qteCaller == QTEManager.Caller.Crafting && QTEManager.Instance._isSuccess == 1)
        {
            //Success
            QTEManager.Instance._isSuccess = 0;
            Deselect();
            _craftActive = false;

            _crafting.AddIngridient(_itemType);
            _canvasGroup.DOFade(.5f, .4f).SetDelay(_slideUpTime);
            _interactable = false;
        }

        if (_qteCaller == QTEManager.Caller.Crafting && QTEManager.Instance._isSuccess == -1)
        {
            //Fail
            QTEManager.Instance._isSuccess = 0;
            Deselect();
            _craftActive = false;
            _interactable = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_interactable == false) return;
        if (_crafting.GetNextCraftingStep() != _itemType)
        {
            _interactable = false;
            transform.DOShakePosition(_shakeDuration, _shakeDirection, 10, 0, false, true, ShakeRandomnessMode.Harmonic).OnComplete(() => _interactable = true);
            return;
        }

        if (_selected == false)
        {
            Select();
            QTEManager.Instance.QTEStart(QTEManager.Caller.Crafting, _qteDuration);
            _craftActive = true;
        }
    }

    public void SetItem(Item.ItemType type)
    {
        if (type != Item.ItemType.None)
        {
            _itemType = type;
            _itemImage.sprite = ItemRegister.Instance.items.Find(x => x.itemType == _itemType).sprite;
        }
        else 
        {
            _itemType = type;
            _itemImage.sprite = null;
        }
    }

    public void Reset()
    {
        _itemType = Item.ItemType.None;
        _itemImage.sprite = null;
        _canvasGroup.alpha = 1f;
        _interactable = true;
        _selected = false;
        _craftActive = false;

        //QTE reset action needed for unplanned end of QTE
    }

    private void Select()
    {
        transform.DOLocalMoveY(transform.localPosition.y + _slideUp, _slideUpTime);
        _selected = true;
        _interactable = false;
    }

    private void Deselect()
    {
        transform.DOLocalMoveY(transform.localPosition.y - _slideUp, _slideUpTime);
        _selected = false;
    }
}
