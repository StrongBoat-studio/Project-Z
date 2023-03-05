using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Item.ItemType _itemType = Item.ItemType.None;
    [SerializeField] private Image _itemImage;
    private Vector3 _startPos;
    private int _slideUp = 40;
    private bool _selected = false;
    private bool _craftActive = false;
    private QTEManager.Caller _caller;
    private CanvasGroup _canvasGroup;
    private bool _interactable = true;
    [SerializeField] private Crafting _crafting;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
        _startPos = transform.position;
        _canvasGroup = GetComponent<CanvasGroup>();

        _itemImage.sprite = ItemRegister.Instance.items.Find(x => x.itemType == _itemType).sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(_craftActive == false) return;
        _caller=QTEManager.Instance.QTEAction(QTEManager.Caller.Crafting, 1);

        if (_caller == QTEManager.Caller.Crafting && QTEManager.Instance._isSuccess == 1)
        {
            //Success
            QTEManager.Instance._isSuccess = 0;
            Deselect();
            _craftActive = false;

            _crafting.AddIngridient(_itemType);
            _canvasGroup.alpha = .5f;
            _interactable = false;
        }

        if (_caller == QTEManager.Caller.Crafting && QTEManager.Instance._isSuccess == -1)
        {
            //Fail
            QTEManager.Instance._isSuccess = 0;
            Deselect();
            _craftActive = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_interactable == false) return;
        if(_crafting.GetNextCraftingStep() != _itemType)
        {
            transform.DOShakePosition(.4f, Vector3.right * 8, 10, 0, false, true, ShakeRandomnessMode.Harmonic);
            return;
        }
        //Mouse click selects current slot ↑
        //and calls qte ↑
        //qte makes a callback with successful/failed result ↑
        //Succes => Add ingridient to current recipe ↑
        //Failed => deselect current slot to try again ↑
        if(_selected == false)
        {
            Select();
            QTEManager.Instance.QTEStart(QTEManager.Caller.Crafting, 5f);
            _craftActive = true;
        }
    }

    private void Select()
    {
        transform.DOLocalMoveY(transform.localPosition.y + _slideUp, .2f);
        _selected = true;
    }

    private void Deselect()
    {
        transform.DOLocalMoveY(transform.localPosition.y - _slideUp, .2f);
        _selected = false;
    }
}
