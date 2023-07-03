using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Animator _animator;
    private GameObject _weaponHolder;
    private int _selectWeapon;
    public bool _weaponHolderActive=true;
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

        if (_playerEarthController.IsEarth())
        {
            _animator.runtimeAnimatorController = _playerEarth;
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
}
