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

    [Header("Mutant movement area")]
    [SerializeField] private int _maxLeftX;
    [SerializeField] private int _maxRightX;

    [Header("Patrol Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed;

    [Header("System check")]
    //Variable for potrolling
    [SerializeField] private int _nextPosition;
    private Vector2 _checkVector = new Vector2(1f, 0f);
    private WalkerBaseState _currensState;
    private System.Random _random = new System.Random();

    [SerializeField] private List<WalkerBaseState> _walkerBaseStates=new List<WalkerBaseState>();


    //controlling bool variables
    private bool _isWalking = false;
    /// <summary>
    /// If true, the mutant must go right, if false, then left
    /// </summary>
    private bool _chectNextPosition = true;

    enum WalkerPatrollingState
    {
        Moving=1,
        Standing=2,
    }

    void Awake()
    {
        //Default to the first position
        _nextPosition = _maxRightX;

        //downloading the appropriate components
        _walkerStateManager = GetComponent<WalkerStateManager>();
        _mutantTransform = GetComponent<Transform>();
        _mutantRigidbody2D = GetComponent<Rigidbody2D>();

        //Adding states to the list
        _walkerBaseStates.Add(_walkerStateManager.PatrollingState);
        _walkerBaseStates.Add(_walkerStateManager.BeforeChaseState);
        _walkerBaseStates.Add(_walkerStateManager.HearingState);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _currensState = _walkerStateManager.GetWalkerState();

        if (_walkerBaseStates.Contains(_currensState))
        {
            WalkerPatrollingState walkerPatrollingState = WalkerPatrollingState.Moving;

            if (_isWalking) walkerPatrollingState = WalkerPatrollingState.Moving;
            if (!_isWalking) walkerPatrollingState = WalkerPatrollingState.Standing;

            switch (walkerPatrollingState)
            {
                case WalkerPatrollingState.Moving:
                    {
                        SetPosition();
                        Moving();

                        break;
                    }

                case WalkerPatrollingState.Standing:
                    {
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
            _nextPosition = _random.Next((int)_mutantTransform.position.x + 1, _maxRightX);

            return _nextPosition;
        }

        return 0;
    }

    private void SetPosition()
    {
        if(_chectNextPosition)
        {
            _checkVector = new Vector2(-1f, 0f);
            _mutantTransform.localScale = new Vector3(-1, 1, 0);

            if(_mutantTransform.position.x >= _nextPosition)
            {
                _nextPosition = DrawNextPosition();
            }
        }
        else if (!_chectNextPosition)
        {
            _checkVector = new Vector2(1f, 0f);
            _mutantTransform.localScale = new Vector3(1, 1, 0);

            if(_mutantTransform.position.x <= _nextPosition)
            {
                _nextPosition = DrawNextPosition();
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

    /// <returns>Returns the vector needed for the dot product in the walker state machine</returns>
    public Vector2 GetCheckVector()
    {
        return _checkVector;
    }
}
