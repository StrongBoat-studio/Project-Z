using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    private Vector3 _startPos;
    private int _slideUp = 40;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Mouse click selects current slot
        //and calls qte
        //qte makes a callback with successful/failed result
        //Succes => Add ingridient to current recipe
        //Failed => deselect current slot to try again
        if(transform.position == _startPos)
        {
            transform.DOLocalMoveY(transform.localPosition.y + _slideUp, .2f);

        }
        else
        {
            transform.DOLocalMoveY(transform.localPosition.y - _slideUp, .2f);
        }
    }
}
