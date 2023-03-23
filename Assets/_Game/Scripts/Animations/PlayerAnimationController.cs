using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    private Transform _transform;
    private Animator _animator;

    [SerializeField] private float _side;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(_movement.GetMovementStates().Contains(Movement.MovementState.Walking))
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false); ;
        }

        
    }

}
