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

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }
    public void CursorClick()
    {
        _playerSprite = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        _playerWeaponHolder = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;
        _movement = GameManager.Instance.movement;

        //Locker enter/exit audio
        if(FMODEvents.Instance != null)
        {
            AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.LockerHide, transform.position);
        }

        if (_playerSprite.activeSelf==true)
        {
            _playerSprite.SetActive(false);
            _playerWeaponHolder.SetActive(false);
            _movement.InLocker(true);
            _movement.enabled = false;
        }
        else
        {
            _playerSprite.SetActive(true);
            _playerWeaponHolder.SetActive(true);
            _movement.enabled = true;
            _movement.InLocker(false);
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
