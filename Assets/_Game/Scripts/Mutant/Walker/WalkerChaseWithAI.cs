using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WalkerChaseWithAI : MonoBehaviour
{
    //Components use for chase
    private WalkerStateManager _walkerStateManager;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;


    [Header("Chase Settings")]
    [Range(0f, 1000f)] [SerializeField] private float _speed;


    //Variable for chase
    private WalkerBaseState _currensState;
    private List<WalkerBaseState> _walkerBaseStates = new List<WalkerBaseState>();

    //Variables for AI
    private Transform _target;
    private float _nextWaypointDistance = 3f;
    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    private Vector2 _direction;
    private Vector2 _force;
    private float _distance;

    private void Awake()
    {
        //downloading the appropriate components
        _walkerStateManager = GetComponent<WalkerStateManager>();

        //Adding states to the list
        _walkerBaseStates.Add(_walkerStateManager.ChaseState);
        _target= GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //Start Path generate
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if(_seeker.IsDone())
        {
            _seeker.StartPath(_rigidbody2D.position, _target.position, OnPathComplete);
        }
    }

    private void FixedUpdate()
    {
        _currensState = _walkerStateManager.GetWalkerState();

        if(_walkerBaseStates.Contains(_currensState) && _path!=null)
        {
            if(_currentWaypoint>=_path.vectorPath.Count)
            {
                _reachedEndOfPath = true;
                return;
            }
            else
            {
                _reachedEndOfPath = false;
            }

            Chase();
            DistanceCalculation();
            IsNextWaypoint();
        }
    }

    private void Chase()
    {
        _direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;
        _force = _direction * _speed * Time.deltaTime;

        _rigidbody2D.AddForce(_force);
    }

    private void DistanceCalculation()
    {
        _distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);
    }

    private void IsNextWaypoint()
    {
        if(_distance<_nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }

    private void OnPathComplete(Path path)
    {
        if(!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }
}
