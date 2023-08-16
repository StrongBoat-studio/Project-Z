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
    private PlayerEarthController _playerEarthController;

    private int _movementState = (int)MovementState.Standing; //State of the player, represented as bits 
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
    [SerializeField] private Image _uiStaminaBar;
    [Range(0f, 10f)][SerializeField] private float _staminaMax;
    [Range(0f, 10f)][SerializeField] private float _staminaDrainSpeed;
    [Range(0f, 10f)][SerializeField] private float _staminaRecoveryDelay;
    [Range(0f, 10f)][SerializeField] private float _staminaRecoverySpeed;
    [Range(0f, 10f)][SerializeField] private float _staminaDrainJump;
    [Range(0f, 10f)][SerializeField] private float _crouchTimeout;

    private bool _isWalking = false;
    private bool _isRunning = false;
    private bool _isCrouching = false;
    private bool _isCreeping = false;
    private bool _isJumping = false;

    private float _staminaCurrent;
    private float _staminaRecoveryCurrentTime;
    private float _crouchTimer = 0f;

    public bool IsDoorAnimationPlay = false;
    public bool canMove = true;
    private Vector2 _positionDuringDoorAnimation;
    #endregion

    public MovementState[] ms;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
        _playerEarthController = GetComponent<PlayerEarthController>();
        _staminaCurrent = _staminaMax;
        _staminaRecoveryCurrentTime = _staminaRecoveryDelay;

        //Init player input
        _playerInput = new PlayerInput();
        _playerInput.InGame.Enable();

        _playerInput.InGame.Walk.performed += _ => _isWalking = true;
        _playerInput.InGame.Walk.canceled += _ => _isWalking = false;

        _playerInput.InGame.Run.started += _ => _isRunning = true;
        _playerInput.InGame.Run.canceled += _ => _isRunning = false;

        _playerInput.InGame.Creep.performed += _ => _isCreeping = true;
        _playerInput.InGame.Creep.canceled += _ => _isCreeping = false;

        _playerInput.InGame.Jump.performed += OnJump;

        _playerInput.InGame.Crouch.performed += _ => _isCrouching = true;
        _playerInput.InGame.Crouch.canceled += _ => _isCrouching = false;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    } 

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        _playerInput.InGame.Jump.performed -= OnJump;
    }

    private void Update()
    {
        if(_crouchTimer > 0f) _crouchTimer -= Time.deltaTime;

        CalculateState();

        CalculateStaminaChange();

        ChangeSide();

        PositionDuringDoorAnimation();
            
    }

    private void FixedUpdate()
    {
        List<MovementState> currentStates = GetMovementStates();
        ms = currentStates.ToArray();

        //Check Jump
        if (currentStates.Contains(MovementState.Jumping))
        {
            if (IsGrounded() == true) AlterMovementState(0, MovementState.Jumping);
        }

        CalculateMovementSpeed();

        if(IsDoorAnimationPlay==false || canMove==true)
        {
            _rigidbody.velocity = new Vector2(_movementSpeedCalculated, _rigidbody.velocity.y);
        }
    }

    private void CalculateState()
    {
        List<MovementState> states = GetMovementStates();

        // Walking
        if (
            states.Contains(MovementState.Walking) == false &&
            _isWalking == true &&
            IsGrounded() == true &&
            IsDoorAnimationPlay == false &&
            canMove == true
        ) 
        {
            AlterMovementState(MovementState.Walking, 0);
        }
        else if (
            _isWalking == false ||
            IsGrounded() == false
        )
        {
            // If player is not walking, he can't run or creep
            AlterMovementState(0, MovementState.Walking); 
            AlterMovementState(0, MovementState.Running); 
            AlterMovementState(0, MovementState.Creeping);
        }


        // Running
        if(
            states.Contains(MovementState.Walking) == true &&
            states.Contains(MovementState.Running) == false &&
            _isRunning == true && 
            IsGrounded() == true
        ) 
        {
            if(states.Contains(MovementState.Crouching) == true && _crouchTimer <= 0f) _crouchTimer = _crouchTimeout; 
            if(!_playerEarthController.IsEarth())
            {
                AlterMovementState(MovementState.Running, 0f);

            }
            AlterMovementState(0f, MovementState.Creeping);
            AlterMovementState(0f, MovementState.Crouching);

            _playerInput.InGame.Creep.Reset(); 
            _playerInput.InGame.Crouch.Reset(); 
        }
        else if (
            _isRunning == false || 
            IsGrounded() == false
        )
        {
            AlterMovementState(0, MovementState.Running);
        }

        // Crouching
        if(
            states.Contains(MovementState.Crouching) == false &&
            _isCrouching == true && 
            IsGrounded() == true &&
            _crouchTimer <= 0f
        )
        {
            if (!_playerEarthController.IsEarth())
                AlterMovementState(MovementState.Crouching, 0f); 
            AlterMovementState(0f, MovementState.Creeping);
            AlterMovementState(0f, MovementState.Running);

            _playerInput.InGame.Creep.Reset(); 
            _playerInput.InGame.Run.Reset(); 
        }
        else if (
            states.Contains(MovementState.Crouching) == true &&
            _isCrouching == true &&
            IsGrounded() == false
        )
        {
            // Player is falling to crouch, do nothing
        }
        else if (
            _isCrouching == false ||
            IsGrounded() == false
        )
        {
            if(states.Contains(MovementState.Crouching) == true && _crouchTimer <= 0f) _crouchTimer = _crouchTimeout; 
            AlterMovementState(0, MovementState.Crouching);
        }

        // Creeping
        if(
            states.Contains(MovementState.Walking) == true &&
            states.Contains(MovementState.Creeping) == false && 
            _isCreeping == true && 
            IsGrounded() == true
        )
        {
            if(states.Contains(MovementState.Crouching) == true && _crouchTimer <= 0f) _crouchTimer = _crouchTimeout; 
            AlterMovementState(MovementState.Creeping, 0f);
            AlterMovementState(0f, MovementState.Crouching);
            AlterMovementState(0f, MovementState.Running);

            _playerInput.InGame.Crouch.Reset(); 
            _playerInput.InGame.Run.Reset(); 
        } 
        else if (
            _isCreeping == false ||
            IsGrounded() == false
        )
        {
            AlterMovementState(0, MovementState.Creeping);
        }
    }

    private void CalculateMovementSpeed()
    {
        //Modify movement speed only when player is on the ground
        if (!IsGrounded()) return;

        if(canMove==false)
        {
            _movementSpeedCalculated = 0;
            AlterMovementState(0, MovementState.Crouching);
            AlterMovementState(0, MovementState.Running);
            AlterMovementState(0, MovementState.Walking);
            return;
        }

        //Left - Right
        float moveRaw = _playerInput.InGame.Walk.ReadValue<float>() * _movementSpeed;

        List<MovementState> states = GetMovementStates();
        if (states.Contains(MovementState.Running))
        {
            //Only sprint when stamina is > 0
            if (_staminaCurrent > 0f)
            {
                //Force relese keys, reset states
                if(states.Contains(MovementState.Crouching))
                    _playerInput.InGame.Crouch.Reset();
                
                if(states.Contains(MovementState.Creeping))
                    _playerInput.InGame.Creep.Reset();

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
            if(states.Contains(MovementState.Running))
                _playerInput.InGame.Run.Reset();
            if(states.Contains(MovementState.Crouching))
                _playerInput.InGame.Crouch.Reset();

            moveRaw *= _creepMultipier;
        }
        else if (states.Contains(MovementState.Crouching))
        {
            //Force relese keys, reset states
            if(states.Contains(MovementState.Creeping))
                _playerInput.InGame.Creep.Reset();
            if(states.Contains(MovementState.Running))
                _playerInput.InGame.Run.Reset();
            moveRaw *= _crouchMultipier;
        }

        //Save movement speed value
        _movementSpeedCalculated = moveRaw;
    }

    private void CalculateStaminaChange()
    {
        //Only regenerate stamina if player is grounded
        if(!IsGrounded()) return;
        if (!canMove) return;

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

        _uiStaminaBar.fillAmount = Mathf.Clamp(_staminaCurrent / _staminaMax, 0f, 1f);
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, 1 / 32f, _groundLayer);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;
        if (!canMove) return;
        if (_staminaCurrent < _staminaDrainJump) return;

        if(!_playerEarthController.IsEarth())
        {
            AlterMovementState(MovementState.Jumping, 0);
            _staminaCurrent -= _staminaDrainJump;
            _rigidbody.AddForce(Vector2.up * _jumpForce * Time.fixedDeltaTime * JUMPFORCE_SCALE, ForceMode2D.Impulse);
        }

        _playerInput.InGame.Crouch.Reset();
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
            newGameState == GameStateManager.GameState.Dialogue ||
            newGameState == GameStateManager.GameState.Loading
        ) _playerInput.InGame.Disable();
        else if(newGameState == GameStateManager.GameState.Gameplay) _playerInput.InGame.Enable();
    }

    private void ChangeSide()
    {

        if(IsDoorAnimationPlay)
        {
            return;
        }

        if(
            Camera.main.ScreenToWorldPoint(_playerInput.InGame.MousePosition.ReadValue<Vector2>()).x >= transform.position.x &&
            transform.localScale.x != -1 && !IsDoorAnimationPlay
        )
        {
            _transform.localScale = new Vector3(-1f, _transform.localScale.y, transform.localScale.z);
        }
        else if(
            Camera.main.ScreenToWorldPoint(_playerInput.InGame.MousePosition.ReadValue<Vector2>()).x < transform.position.x &&
            transform.localScale.x != 1 && !IsDoorAnimationPlay
        )
        {
            _transform.localScale = new Vector3(1f, _transform.localScale.y, transform.localScale.z);
        }

        //Exit running state if player's movement direction is opposite to player's looking direction
        if(
            (GetMovementStates().Contains(MovementState.Running) && 
            _playerInput.InGame.Walk.ReadValue<float>() == 1f &&
            transform.localScale.x == 1f) ||
            (GetMovementStates().Contains(MovementState.Running) && 
            _playerInput.InGame.Walk.ReadValue<float>() == -1f &&
            transform.localScale.x == -1f)
        )
        {
            AlterMovementState(0, MovementState.Running);
        }
    }

    public void Rotate()
    {
        transform.localScale = new Vector2(-1, transform.localScale.y);
    }

    private void PositionDuringDoorAnimation()
    {
        if (IsDoorAnimationPlay == false)
        {
            _positionDuringDoorAnimation = transform.position;
        }
        else
        {
            transform.position = new Vector2(_positionDuringDoorAnimation.x, transform.position.y);
        }
    }

    public float GetStamina()
    {
        return _staminaCurrent;
    }

    public void SetStamina(float stamina)
    {
        _staminaCurrent = stamina;
    }

    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void OnDisable()
    {
        _isWalking = false;
        AlterMovementState(0, MovementState.Walking);
    }
}
