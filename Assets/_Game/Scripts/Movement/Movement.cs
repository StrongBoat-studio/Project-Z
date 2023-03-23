using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private Transform _transform;

    private int _movementState = (int)MovementState.Standing; //State of the player, represented ad bits 
    private float _movementSpeedCalculated = 0f;

    #region Movement variables
    [Header("Movement")]
    [SerializeField] private LayerMask _groundLayer;
    [Range(0f, 10f)][SerializeField] private float _movementSpeed;
    [Range(0f, 10f)][SerializeField] private float _crouchMultipier;
    [Range(0f, 10f)][SerializeField] private float _runMultiplier;
    [Range(0f, 10f)][SerializeField] private float _creepMultipier;
    [Range(0f, 10f)][SerializeField] private float _jumpForce;
    private const float JUMPFORCE_SCALE = 10000f;
    [SerializeField] private Slider _uiStaminaBar;
    [Range(0f, 10f)][SerializeField] private float _staminaMax;
    [Range(0f, 10f)][SerializeField] private float _staminaDrainSpeed;
    [Range(0f, 10f)][SerializeField] private float _staminaRecoveryDelay;
    [Range(0f, 10f)][SerializeField] private float _staminaRecoverySpeed;
    [Range(0f, 10f)][SerializeField] private float _staminaDrainJump;
    private float _staminaCurrent;
    private float _staminaRecoveryCurrentTime;
    #endregion

    public MovementState[] ms;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
        _staminaCurrent = _staminaMax;
        _staminaRecoveryCurrentTime = _staminaRecoveryDelay;

        //Init player input
        _playerInput = new PlayerInput();
        _playerInput.InGame.Enable();

        _playerInput.InGame.Walk.performed +=
            ctx => AlterMovementState(MovementState.Walking, 0);
        _playerInput.InGame.Walk.canceled +=
            ctx => AlterMovementState(0, MovementState.Walking);

        _playerInput.InGame.Run.started +=
            ctx => AlterMovementState(MovementState.Running, 0);
        _playerInput.InGame.Run.canceled +=
            ctx => AlterMovementState(0, MovementState.Running);

        _playerInput.InGame.Creep.performed +=
            ctx => AlterMovementState(MovementState.Creeping, 0);
        _playerInput.InGame.Creep.canceled +=
            ctx => AlterMovementState(0, MovementState.Creeping);

        _playerInput.InGame.Jump.performed += OnJump;

        _playerInput.InGame.Crouch.performed +=
            ctx => AlterMovementState(MovementState.Crouching, 0);
        _playerInput.InGame.Crouch.canceled +=
            ctx => AlterMovementState(0, MovementState.Crouching);

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.InGame.Jump.performed -= OnJump;
    }

    private void Update()
    {
        CalculateStaminaChange();

        ChangeSide();
    }

    private void FixedUpdate()
    {
        List<MovementState> currentStates = GetMovementStates();
        ms = currentStates.ToArray();

        //Check Jump
        if (currentStates.Contains(MovementState.Jumping))
        {
            if (IsGrounded()) AlterMovementState(0, MovementState.Jumping);
        }

        CalculateMovementSpeed();
        _rigidbody.velocity = new Vector2(_movementSpeedCalculated, _rigidbody.velocity.y);
    }

    private void CalculateMovementSpeed()
    {
        //Modify movement speed only when player is on the ground
        if (!IsGrounded()) return;

        //Left - Right
        float moveRaw = _playerInput.InGame.Walk.ReadValue<float>() * _movementSpeed;

        List<MovementState> states = GetMovementStates();
        if (states.Contains(MovementState.Running))
        {
            //Only sprint when stamina is > 0
            if (_staminaCurrent > 0f)
            {
                //Force relese keys, reset states
                _playerInput.InGame.Creep.Reset();
                _playerInput.InGame.Crouch.Reset();

                moveRaw *= _runMultiplier;

                //Consume stamina if sprinting
                if (states.Contains(MovementState.Walking))
                    _staminaCurrent -= Time.deltaTime * _staminaDrainSpeed;
            }
            else
            {
                AlterMovementState(0, MovementState.Running);
                _playerInput.InGame.Run.Reset();
                _staminaRecoveryCurrentTime = 0f;
            }
        }
        else if (states.Contains(MovementState.Creeping))
        {
            //Force relese keys, reset states
            _playerInput.InGame.Run.Reset();
            _playerInput.InGame.Crouch.Reset();

            moveRaw *= _creepMultipier;
        }
        else if (states.Contains(MovementState.Crouching))
        {
            //Force relese keys, reset states
            _playerInput.InGame.Creep.Reset();
            _playerInput.InGame.Run.Reset();

            moveRaw *= _crouchMultipier;
        }

        //Save movement speed value
        _movementSpeedCalculated = moveRaw;
    }

    private void CalculateStaminaChange()
    {
        List<MovementState> states = GetMovementStates();

        if (_staminaCurrent <= 0f && _staminaRecoveryCurrentTime < _staminaRecoveryDelay)
        {
            _staminaRecoveryCurrentTime += Time.deltaTime;
            _staminaCurrent = 0f;
        }
        else if (
            _staminaRecoveryCurrentTime >= _staminaRecoveryDelay &&
            !states.Contains(MovementState.Jumping) &&
            (!states.Contains(MovementState.Walking) || !states.Contains(MovementState.Running))
        )
        {
            _staminaCurrent += Time.deltaTime * _staminaRecoverySpeed;
            if (_staminaCurrent >= _staminaMax)
                _staminaCurrent = _staminaMax;
        }

        _uiStaminaBar.value = Mathf.Clamp(_staminaCurrent / _staminaMax, 0f, 1f);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, 1 / 32f, _groundLayer);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;
        if (_staminaCurrent < _staminaDrainJump) return;

        _staminaCurrent -= _staminaDrainJump;
        _rigidbody.AddForce(Vector2.up * _jumpForce * Time.fixedDeltaTime * JUMPFORCE_SCALE, ForceMode2D.Impulse);
    }

    ///<summary>
    ///Adds and removes given movement states
    ///</summary>
    ///<param name="add">States to add</param>
    ///<param name="remove">States to remove</param>
    private void AlterMovementState(MovementState add, MovementState remove)
    {
        _movementState |= (int)add;
        _movementState &= ~(int)remove;
    }

    /// <summary>
    /// Get all active movement states
    /// </summary>
    /// <returns>Returns list of all movement states that are currently active</returns>
    public List<MovementState> GetMovementStates()
    {
        List<MovementState> states = new List<MovementState>();

        //Loop throgh all MovementStates
        for (int i = 1; i < (int)Mathf.Pow(2, Enum.GetNames(typeof(MovementState)).Length); i <<= 1)
        {
            //If MovementState flag is set, add it to list
            if ((_movementState & i) == i) states.Add((MovementState)i);
        }

        return states;
    }

    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if(
            newGameState == GameStateManager.GameState.Paused ||
            newGameState == GameStateManager.GameState.Dialogue
        ) _playerInput.InGame.Disable();
        else if(newGameState == GameStateManager.GameState.Gameplay) _playerInput.InGame.Enable();
    }


    void ChangeSide()
    {
        if (_playerInput.InGame.Walk.ReadValue<float>() < 0)
        {
            _transform.localScale = new Vector3(1, _transform.localScale.y, _transform.localScale.z);
        }
        else if (_playerInput.InGame.Walk.ReadValue<float>() > 0)
        {
            _transform.localScale = new Vector3(-1, _transform.localScale.y, _transform.localScale.z);
        }
    }
}
