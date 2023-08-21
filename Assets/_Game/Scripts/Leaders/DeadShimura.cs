using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class DeadShimura : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;
    [SerializeField] private Item _gun;
    [SerializeField] private Item _note;

    private Player _player;
    private bool _giveItem;
    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    private void Update()
    {
        if(QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Argue with Boris")
        {
            Destroy(this);
        }
    }

    public void CursorClick()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(_player!=null)
        {
            _player.AddItem(_gun);
            _player.AddItem(_note);
        }

        Destroy(this);
    }

    public void CursorEnter(bool canInteract)
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
        GetComponent<SpriteRenderer>().material.SetColor("_Color", canInteract ? _canInteractColor : _cannotInteractColor);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }
}
