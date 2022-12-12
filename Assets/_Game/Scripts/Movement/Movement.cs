using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
    //For new states use next powers of 2
    public enum MovementState
    {
        Standing = 1,
        Crouching = 2,
        Creeping = 4,
        Walking = 8,
        Running = 16,
        Jumping = 32,
    }

    private PlayerInput _playerInput;
    private int _movementState = (int)MovementState.Standing; //State of the player, represented ad bits 
    private Rigidbody2D _rigidbody;
    private bool _onGround = true;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _crouchSpeedMultipier;
    [SerializeField] private float _runSpeedMultiplier;
    [SerializeField] private float _creepSpeedMultipier;
    [SerializeField] private float _jumpForce;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        //Init player input
        _playerInput = new PlayerInput();
        _playerInput.InGame.Enable();
        _playerInput.InGame.Walk.performed += OnWalkPerformed;
        _playerInput.InGame.Walk.canceled += OnWalkCanceled;
        _playerInput.InGame.Run.performed += OnRunPerformed;
        _playerInput.InGame.Run.canceled += OnRunCanceled;
        _playerInput.InGame.Creep.performed += OnCreepPerformed;
        _playerInput.InGame.Creep.canceled += OnCreepCanceled;
        _playerInput.InGame.Jump.performed += OnJump;
        _playerInput.InGame.Crouch.performed += OnCrouchPerformed;
        _playerInput.InGame.Crouch.canceled += OnCrouchCanceled;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //Calculate movement speed
        //Left - Right movment, read every frame
        float moveRaw = _playerInput.InGame.Walk.ReadValue<float>() * _walkSpeed;

        List<MovementState> currentStates = GetMovementStates();

        //Modify movement speed only when player is on the ground
        if (_onGround)
        {
            if (currentStates.Contains(MovementState.Running))
            {
                //Run => unmset creeping and crouching
                _movementState &= ~((int)MovementState.Creeping | (int)MovementState.Crouching);

                //Force relese keys
                _playerInput.InGame.Creep.Reset();
                _playerInput.InGame.Crouch.Reset();

                moveRaw *= _runSpeedMultiplier;
            }
            else if (currentStates.Contains(MovementState.Creeping))
            {
                //Creep => unset crouching and running
                _movementState &= ~((int)MovementState.Crouching | (int)MovementState.Running);

                //Force relese keys
                _playerInput.InGame.Run.Reset();
                _playerInput.InGame.Crouch.Reset();

                moveRaw *= _creepSpeedMultipier;
            }
            else if (currentStates.Contains(MovementState.Crouching))
            {
                //Crouching => unsert creeping and running
                _movementState &= ~((int)MovementState.Creeping | (int)MovementState.Running);

                //Force relese keys
                _playerInput.InGame.Creep.Reset();
                _playerInput.InGame.Run.Reset();

                moveRaw *= _crouchSpeedMultipier;
            }
        }

        //Apply movement speed
        _rigidbody.velocity = new Vector2(moveRaw * Time.fixedDeltaTime, _rigidbody.velocity.y);
    }

    private void OnWalkPerformed(InputAction.CallbackContext context)
    {
        //Set walking state
        _movementState |= (int)MovementState.Walking;
    }

    private void OnWalkCanceled(InputAction.CallbackContext context)
    {
        //Remove walking state
        _movementState &= ~(int)MovementState.Walking;
    }

    private void OnRunPerformed(InputAction.CallbackContext context)
    {
        //Set running state
        _movementState |= (int)MovementState.Running;

        //Remove creeping state
        _movementState &= ~(int)MovementState.Creeping;
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        //Remove running state
        _movementState &= ~(int)MovementState.Running;
    }

    private void OnCreepPerformed(InputAction.CallbackContext context)
    {
        //Set creeping state
        _movementState |= (int)MovementState.Creeping;

        //Remove running state
        _movementState &= ~(int)MovementState.Running;
    }

    private void OnCreepCanceled(InputAction.CallbackContext context)
    {
        //Remove creeping state
        _movementState &= ~(int)MovementState.Creeping;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_onGround) return;

        _rigidbody.AddForce(Vector2.up * _jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        //Set crouching state
        _movementState |= (int)MovementState.Crouching;
        transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
        GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
    }

    private void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        //Remove crouching state
        _movementState &= ~(int)MovementState.Crouching;
        transform.GetChild(0).transform.localScale = new Vector3(1, 2, 1);
        GetComponent<BoxCollider2D>().size = new Vector2(1, 2);
    }

    //Collisions with ground objects to reset jump ability
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        if (!GetMovementStates().Contains(MovementState.Jumping)) return;
        
        //Set jumping state
        _movementState &= ~(int)MovementState.Jumping;
        _onGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        if (GetMovementStates().Contains(MovementState.Jumping)) return;

        //Remove jumping state
        _movementState |= (int)MovementState.Jumping;
        _onGround = false;
    }

    /// <summary>
    /// Get all active movement states
    /// </summary>
    /// <returns>Returns list of all movement states that are currently active</returns>
    public List<MovementState> GetMovementStates()
    {
        List<MovementState> states = new List<MovementState>();

        //Loop throgh all MovementStates
        for(int i = 1; i < (int)Mathf.Pow(2, Enum.GetNames(typeof(MovementState)).Length); i <<= 1)
        {
            //If MovementState flag is set, add it to list
            if ((_movementState & i) == i) states.Add((MovementState)i);
        }

        return states;
    }
}
