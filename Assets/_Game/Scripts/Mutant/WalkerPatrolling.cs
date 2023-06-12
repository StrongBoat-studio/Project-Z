using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerPatrolling : MonoBehaviour
{
    [Header("Object use for patrolling")]
    [SerializeField] private WalkerStateManager _walkerStateManager;
    [SerializeField] private Transform _mutantTransform;
    [SerializeField] private Rigidbody2D _mutantRigidbody2D;

    [Header("Mutant movement area")]
    [SerializeField] private float _maxLeftX;
    [SerializeField] private float _maxRightX;

    [Header("Patrol Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed;

    //Variable for potrolling
    private float _nextPosition;
    private Vector2 _checkVector = new Vector2(1f, 0f);
    private WalkerBaseState _currensState;

    [SerializeField] private List<WalkerBaseState> _walkerBaseStates;

    // Start is called before the first frame update
    void Awake()
    {
        _nextPosition = _maxRightX;

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
            SetPosition();
            Moving();
        }
       
    }

    private void SetPosition()
    {
        if (_mutantTransform.position.x >= _maxRightX)
        {
            _nextPosition = _maxLeftX;
            _checkVector = new Vector2(1f, 0f);
            _mutantTransform.localScale = new Vector3(1, 1, 0);
        }

        if (_mutantTransform.position.x <= _maxLeftX)
        {
            _nextPosition = _maxRightX;
            _checkVector = new Vector2(-1f, 0f);
            _mutantTransform.localScale = new Vector3(-1, 1, 0);
        }
    }

    private void Moving()
    {
        if (_nextPosition == _maxRightX)
        {
            _mutantRigidbody2D.velocity = new Vector2(_speed, _mutantTransform.position.y);
        }
        else
        {
            _mutantRigidbody2D.velocity = new Vector2(-_speed, _mutantTransform.position.y);
        }

    }

    public Vector2 GetCheckVector()
    {
        return _checkVector;
    }
}
