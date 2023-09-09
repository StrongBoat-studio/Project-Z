using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossChaseWithAI : MonoBehaviour
{
    //Components use for chase
    private BossStateManager _bossStateManager;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;


    [Header("Chase Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed = 3.0f;


    //Variable for chase
    private BossBaseState _currensState;
    private List<BossBaseState> _bossBaseStates = new List<BossBaseState>();

    //Variables for AI
    private Transform _target;
    private float _nextWaypointDistance = 2f;
    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    private Vector2 _direction;
    private float _distance;

    private void Awake()
    {
        //downloading the appropriate components
        _bossStateManager = GetComponent<BossStateManager>();
        _target = GameManager.Instance.player;
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //Adding states to the list
        _bossBaseStates.Add(_bossStateManager.ChaseState);
        _bossBaseStates.Add(_bossStateManager.AttackState);

        //Start Path generate
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if (_target == null)
        {
            _target = GameManager.Instance.player;

            if (_target == null) return;
        }

        if (_seeker.IsDone() && this.gameObject.activeSelf == true)
        {
            _seeker.StartPath(_rigidbody2D.position, _target.position, OnPathComplete);
        }
    }

    private void FixedUpdate()
    {
        _currensState = _bossStateManager.GetBossState();

        if (_path == null) return;

        if (_bossBaseStates.Contains(_currensState))
        {
            if (_currentWaypoint >= _path.vectorPath.Count)
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
            RotatingMutant();
        }
    }

    private void Chase()
    {
        _direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;

        _rigidbody2D.velocity = new Vector2(_speed * _direction.x, _rigidbody2D.position.y);
    }

    private void DistanceCalculation()
    {
        _distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);
    }

    private void IsNextWaypoint()
    {
        if (_distance < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    private void RotatingMutant()
    {
        float x = transform.localScale.x;

        if (_rigidbody2D.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (_rigidbody2D.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (x != transform.localScale.x)
        {
            AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantGrowl, this.gameObject.transform.position);
        }
    }
}
