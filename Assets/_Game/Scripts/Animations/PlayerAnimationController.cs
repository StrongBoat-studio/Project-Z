using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    private Animator _animator;

    [SerializeField] private GameObject _weaponHolder;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        IsWalking();
        IsSprint();
        IsCrouching();
        IsJumping();
    }

    private void IsWalking()
    {
        if (_movement.GetMovementStates().Contains(Movement.MovementState.Walking))
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false); ;
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
            _weaponHolder.SetActive(false);
            _animator.SetBool("IsCrounch", true);
        }
        else
        {
            _weaponHolder.SetActive(true);
            _animator.SetBool("IsCrounch", false); ;
        }
    }

    private void IsJumping()
    {
        if(!_movement.IsGrounded())
        {
            _animator.SetBool("IsJumping", true);
        }
        else
        {
            _animator.SetBool("IsJumping", false);
        }

    }
}
