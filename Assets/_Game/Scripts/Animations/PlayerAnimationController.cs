using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Movement _movement;

    [Header("Runtime Animator Controller")]
    [SerializeField] private RuntimeAnimatorController _player;
    [SerializeField] private RuntimeAnimatorController _playerWH;
    [SerializeField] private RuntimeAnimatorController _playerEarth;

    [Header("Weapon")]
    [SerializeField] private WeaponSwitching _weaponSwitching;
    [SerializeField] private GameObject _gun;
    [SerializeField] private Animator _animGun;

    [Header("Earth Controller")]
    [SerializeField] private PlayerEarthController _playerEarthController;

    [SerializeField] private Animator _locationLoader;

    private Animator _animator;
    private GameObject _weaponHolder;
    private int _selectWeapon;
    public bool _weaponHolderActive=true;
    public bool _isDoorAnimationPlay = false;
    private Vector2 _gunPosition = new Vector2(0f, 0f);

    void Awake()
    {
        //Downloading the appropriate components
        _animator = GetComponent<Animator>();
        _weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder");

        _gun.transform.position = _gunPosition;
    }

    // Update is called once per frame
    void Update()
    {

        IsWalking();
        WeaponHolderActive(_weaponHolderActive);
        _movement.IsDoorAnimationPlay = _isDoorAnimationPlay;

        if (_playerEarthController.IsEarth())
        {
            _animator.runtimeAnimatorController = _playerEarth;

            try
            {
                _locationLoader = GameObject.FindGameObjectWithTag("RoomLoader").GetComponentInChildren<Animator>();
            }
            catch
            {
                _locationLoader = null;
            }

            return;
        }

        SetAnimatiorController();

        IsJumping();
        IsSprint();
        IsCrouching();
    }

    private void SetAnimatiorController()
    {
        _selectWeapon = _weaponSwitching.GetWeapon();

        if (_selectWeapon == 1 || _selectWeapon == 2)
        {
            _animator.runtimeAnimatorController = _playerWH;
            _weaponHolderActive = true;
        }
        else
        {
            _animator.runtimeAnimatorController = _player;
            _weaponHolderActive = true;
        }
    }

    private void WeaponHolderActive(bool _active)
    {
        _weaponHolder.SetActive(_active);
    }

    private void IsWalking()
    {
        if (_movement.GetMovementStates().Contains(Movement.MovementState.Walking))
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }

    private void IsSprint()
    {
        if (_movement.GetMovementStates().Contains(Movement.MovementState.Running))
        {
            _animator.SetBool("IsSprint", true);
        }
        else
        {
            _animator.SetBool("IsSprint", false); ;
        }
    }

    private void IsCrouching()
    {
        if (_movement.GetMovementStates().Contains(Movement.MovementState.Crouching))
        {   
            _animator.SetBool("IsCrounch", true);
        }
        else
        {
            _animator.SetBool("IsCrounch", false);
        }
    }

    private void IsJumping()
    {
        if(!_movement.IsGrounded() && !_movement.GetMovementStates().Contains(Movement.MovementState.Crouching))
        {
            _animator.SetBool("IsJumping", true);
            _animGun.SetBool("IsJumping", true);
        }
        else
        {
            _animator.SetBool("IsJumping", false);
            _animGun.SetBool("IsJumping", false);
        }
    }

    /// <summary>
    /// Setting the gun position according to the animation
    /// </summary>
    private void SettingTheGunPosition()
    {
        _gun.transform.position = new Vector2(_gun.transform.position.x+_gunPosition.x, _gun.transform.position.y + _gunPosition.y);
    }

    public void RoomLoaderAnimStart()
    {
        if(_locationLoader != null)
        {
            _locationLoader.SetBool("IsOpen", true);
        }
    }

    public void Rotation()
    {
        GameManager.Instance.movement.Rotate();
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        _animator.SetBool("IsTriggerDoor2", false);
    }
}
