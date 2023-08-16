using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class LockerController : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;
    private GameObject _playerSprite;
    private GameObject _playerWeaponHolder;
    private Movement _movement;
    private bool _isPlayerInThisLocker = false;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }
    public void CursorClick()
    {
        _playerSprite = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        _playerWeaponHolder = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;
        _movement = GameManager.Instance.movement;

        if (_playerSprite.activeSelf==true)
        {
            _movement.CanMove(false);
            _playerSprite.SetActive(false);
            _playerWeaponHolder.SetActive(false);
            _isPlayerInThisLocker = true;
        }
        else if(_isPlayerInThisLocker)
        {
            _playerSprite.SetActive(true);
            _playerWeaponHolder.SetActive(true);
            _movement.CanMove(true);
            _isPlayerInThisLocker = false;
        }
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
