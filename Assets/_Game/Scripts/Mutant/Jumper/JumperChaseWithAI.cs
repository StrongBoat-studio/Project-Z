using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class JumperChaseWithAI : MonoBehaviour
{
    //Components use for chase
    private JumperStateManager _jumperStateManager;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;


    [Header("Chase Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed = 4.0f;

    //Variable for chase
    private JumperBaseState _currensState;
    private List<JumperBaseState> _jumperBaseStates = new List<JumperBaseState>();

    //Variables for AI
    private Transform _target;
    private Transform _targetPlayerFrontOfMe;
    private Transform _targetPlayerBackwards;
    private float _nextWaypointDistance = 2f;
    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    [SerializeField] private Vector2 _direction;
    private float _distance;
    
    //DotPro
    [SerializeField]  private float _dotPro;
    private Vector2 _directionDot;
    private Vector2 _checkVector;

    //Jump
    [Range(0f, 100f)] [SerializeField] private int _jumpPower = 15;
    private Transform _groundCheck;
    [SerializeField] private LayerMask _groundlayer;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private Vector2 force;

    private void Awake()
    {
        //downloading the appropriate components
        _jumperStateManager = GetComponent<JumperStateManager>();
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<Transform>();

        _target = GameManager.Instance.player;


        //Adding states to the list
        _jumperBaseStates.Add(_jumperStateManager.ChaseState);
        _jumperBaseStates.Add(_jumperStateManager.AttackState);

        //Start Path generate
        InvokeRepeating("UpdatePath", 0f, 0.5f);
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
            _seeker.StartPath(_rigidbody2D.position, GetTarget(), OnPathComplete);
        }
    }

    private Vector3 GetTarget()
    {
        if (_target == null)
        {
            _target = GameManager.Instance.player;

            if (_target == null) return new Vector3(.0f, .0f, .0f);
        }

        CalculateDotPro();
        _targetPlayerFrontOfMe = _target.GetChild(3).GetChild(0);
        _targetPlayerBackwards = _target.GetChild(3).GetChild(1);
        return (_dotPro < 0) ? _targetPlayerFrontOfMe.position : _targetPlayerBackwards.position;
    }

    private void FixedUpdate()
    {
        _groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<Transform>();
        if(_groundCheck!=null)
        {
            isGrounded = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1.65f, 0.075f), CapsuleDirection2D.Horizontal, 0, _groundlayer);
        }

        _currensState = _jumperStateManager.GetJumperState();

        if (_path == null) return;

        if (_jumperBaseStates.Contains(_currensState))
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

        _rigidbody2D.velocity = new Vector2(_speed * _direction.x, _rigidbody2D.velocity.y);
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
        if(!isGrounded)
        {
            return;
        }
        if (_rigidbody2D.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_rigidbody2D.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void CalculateDotPro()
    {
        _directionDot = transform.position - _target.position;
        _directionDot.Normalize();

        _dotPro = Vector2.Dot(_directionDot, CalculateCheckVector());
    }

    private Vector2 CalculateCheckVector()
    {
        return _checkVector = new Vector2(_target.localScale.x, .0f);
    }

    public void Jump()
    {
        force = Vector2.up * _jumpPower * Time.fixedDeltaTime * 12500f;
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
}
