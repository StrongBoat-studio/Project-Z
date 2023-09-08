using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkerPatrolling : MonoBehaviour
{
    //Components use for patrolling
    private WalkerStateManager _walkerStateManager;
    private Transform _mutantTransform;
    private Rigidbody2D _mutantRigidbody2D;
    private Animator _animator;

    [Header("Mutant movement area")]
    [SerializeField] private int _maxLeftX = 0;
    [SerializeField] private int _maxRightX = 15;

    [Header("Patrol Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed = 2.0f;
    [Range(0f, 10f)] [SerializeField] private int _standingTimeInSeconds = 2;

    //Variable for potrolling
    private int _nextPosition;
    private float _secondsCountdown;

    private Vector2 _checkVector = new Vector2(1f, 0f);
    private WalkerBaseState _currensState;
    private System.Random _random = new System.Random();

    private List<WalkerBaseState> _walkerBaseStates=new List<WalkerBaseState>();
    private List<int> _walking = new List<int>();

    /// <summary>
    /// If true, the mutant must go right, if false, then left
    /// </summary>
    private bool _chectNextPosition = true;

    enum WalkerPatrollingState
    {
        Moving=1,
        Standing=2,
    }

    private WalkerPatrollingState  _walkerPatrollingState = WalkerPatrollingState.Moving;

    void Awake()
    {
        //Default variable values
        _nextPosition = _maxRightX;
        _secondsCountdown = _standingTimeInSeconds;
        _walkerPatrollingState = WalkerPatrollingState.Moving;

        //downloading the appropriate components
        _walkerStateManager = GetComponent<WalkerStateManager>();
        _mutantTransform = GetComponent<Transform>();
        _mutantRigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        //Adding states to the list
        _walkerBaseStates.Add(_walkerStateManager.PatrollingState);
        _walkerBaseStates.Add(_walkerStateManager.BeforeChaseState);
        _walkerBaseStates.Add(_walkerStateManager.HearingState);

        //Adding variables to the list for the draw
        _walking.Add(1);
        _walking.Add(2);
    }

    private void FixedUpdate()
    {
        _currensState = _walkerStateManager.GetWalkerState();

        if (_walkerBaseStates.Contains(_currensState))
        {
            switch (_walkerPatrollingState)
            {
                case WalkerPatrollingState.Moving:
                    {
                        SetPosition();
                        Moving();
                        _animator.SetBool("IsStanding", false);
                        break;
                    }
                case WalkerPatrollingState.Standing:
                    {
                        _animator.SetBool("IsStanding", true);
                        Standing();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }  
        }
    }

    private int DrawNextPosition()
    {
        int _nextPosition;

        if(_chectNextPosition)
        {
            _chectNextPosition = false;
            _nextPosition = _random.Next(_maxLeftX, (int)_mutantTransform.position.x+1);

            return _nextPosition;
        }
        if (!_chectNextPosition)
        {
            _chectNextPosition = true;

            if((int)_mutantTransform.position.x + 1> _maxRightX)
            {
                _nextPosition = _random.Next(_maxLeftX, (int)_mutantTransform.position.x + 1);
            }
            else
            {
                _nextPosition = _random.Next((int)_mutantTransform.position.x + 1, _maxRightX);
            }

            return _nextPosition;
        }

        return 0;
    }

    private void SetPosition()
    {
        if (_chectNextPosition)
        {
            _checkVector = new Vector2(-1f, 0f);
            _mutantTransform.localScale = new Vector3(-1, 1, 0);

            if(_mutantTransform.position.x >= _nextPosition)
            {
                _nextPosition = DrawNextPosition();
                DrawNextState();
            }
        }
        else if (!_chectNextPosition)
        {
            _checkVector = new Vector2(1f, 0f);
            _mutantTransform.localScale = new Vector3(1, 1, 0);

            if(_mutantTransform.position.x <= _nextPosition)
            {
                _nextPosition = DrawNextPosition();
                DrawNextState();
            } 
        }
    }

    private void Moving()
    {
        if (_chectNextPosition)
        {
            _mutantRigidbody2D.velocity = new Vector2(_speed, _mutantTransform.position.y);
        }
        else
        {
            _mutantRigidbody2D.velocity = new Vector2(-_speed, _mutantTransform.position.y);
        }
    }

    private void Standing()
    {
        TimeManipulation();

        if(_secondsCountdown<=0)
        {
            DrawNextState();
        }
    }

    /// <summary>
    /// This function is responsible for whether the mutant's next move will be walking or standing still for a while
    /// </summary>
    private void DrawNextState()
    {
        int _nextStateDraw = _random.Next(1, 4);

        if(_walking.Contains(_nextStateDraw))
        {
            _walkerPatrollingState = WalkerPatrollingState.Moving;
        }
        else
        {
            _walkerPatrollingState = WalkerPatrollingState.Standing;
            _secondsCountdown = _standingTimeInSeconds;
        }
    }

    private void TimeManipulation()
    {
        _secondsCountdown -= Time.deltaTime;
    }

    /// <returns>Returns the vector needed for the dot product in the walker state machine</returns>
    public Vector2 GetCheckVector()
    {
        return _checkVector;
    }

    public void SetRange(int xLeft, int xRight)
    {
        _maxLeftX = xLeft;
        _maxRightX = xRight;
    }
}
