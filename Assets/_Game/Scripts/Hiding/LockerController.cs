using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class LockerController : MonoBehaviour, IInteractable
{
    private LocalKeyword _OUTLINE_ON;
    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;
    [SerializeField] private GameObject _dust;
    private GameObject _playerSprite;
    private GameObject _playerWeaponHolder;
    private Movement _movement;
    private bool _isPlayerInThisLocker = false;
    private float _shakeDuration = 0.4f;
    private Vector2 _shakeDirection = Vector3.right * 0.1f;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }
    public void CursorClick()
    {
        _playerSprite = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        _playerWeaponHolder = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;
        _movement = GameManager.Instance.movement;

        _dust.SetActive(true);
        transform.DOShakePosition(_shakeDuration, _shakeDirection, 10, 0, false, true, ShakeRandomnessMode.Harmonic);
        StartCoroutine(DustStop());

        //Locker enter/exit audio
        if (FMODEvents.Instance != null)
        {
            AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.LockerHide, transform.position);
        }

        if (_playerSprite.activeSelf==true)
        {
            _movement.CanMove(false);
            _playerSprite.SetActive(false);
            _playerWeaponHolder.SetActive(false);
            _isPlayerInThisLocker = true;

            foreach (GameObject mutant in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if(mutant.GetComponent<WalkerStateManager>() != null)
                {
                    mutant.GetComponent<WalkerStateManager>().canAttack = false;
                }

                if (mutant.GetComponent<JumperStateManager>() != null)
                {
                    mutant.GetComponent<JumperStateManager>().canAttack = false;
                }
            }
        }
        else if(_isPlayerInThisLocker)
        {
            _playerSprite.SetActive(true);
            _playerWeaponHolder.SetActive(true);
            _movement.CanMove(true);
            _isPlayerInThisLocker = false;

            foreach (GameObject mutant in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (mutant.GetComponent<WalkerStateManager>() != null)
                {
                    mutant.GetComponent<WalkerStateManager>().canAttack = true;
                }

                if (mutant.GetComponent<JumperStateManager>() != null)
                {
                    mutant.GetComponent<JumperStateManager>().canAttack = true;
                }
            }
        }
    }

    private IEnumerator DustStop()
    {
        yield return new WaitForSeconds(0.5f);

        _dust.SetActive(false);
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
