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

    //Chase points
    private Transform _front;
    private Transform _back;

    [SerializeField] private Vector2 _frontV;
    [SerializeField] private Vector2 _backV;

    //DotPros
    [SerializeField] private float _dotPro;
    private Vector2 _directionDot;
    private Vector2 _checkVector;
    [SerializeField] private float _dotProForRotation;

    //Attack
    public bool canAttack = true;
    private bool _isFront = true; 

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
            _seeker.StartPath(_rigidbody2D.position, GetTarget(), OnPathComplete);
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
            CalculateDotProForRotation();
            RotatingMutant();
            StartAttackAnimationIfCan();

            _frontV = _front.position;
            _backV = _back.position;
        }
    }

    private void StartAttackAnimationIfCan()
    {
        if (canAttack == false || _bossStateManager.GetBossState()!=_bossStateManager.AttackState) return;
        
        if(_isFront==true)
        {
            if (_bossStateManager.nextAttackId == 3)
            {
                _bossStateManager.Animator.SetBool("IsAttacking3", true);
                return;
            }

            if (_bossStateManager.Mutant.position.x >= GetTarget().x)
            {
                SelectAnimation();
            }
        }
        else
        {
            if (_bossStateManager.nextAttackId == 1)
            {
                _bossStateManager.Animator.SetBool("IsAttacking1", true);
                return;
            }

            if (_bossStateManager.Mutant.position.x <= GetTarget().x)
            {
                SelectAnimation();
            }
        }
    }

    private void SelectAnimation()
    {
        switch (_bossStateManager.nextAttackId)
        {
            case 1:
                {
                    _bossStateManager.Animator.SetBool("IsAttacking1", true);
                    canAttack = false;
                    break;
                }
            case 2:
                {
                    _bossStateManager.Animator.SetBool("IsAttacking2", true);
                    canAttack = false;
                    break;
                }
            case 3:
                {
                    _bossStateManager.Animator.SetBool("IsAttacking3", true);
                    canAttack = false;
                    break;
                }
            default:
                {
                    _bossStateManager.Animator.SetBool("IsAttacking1", true);
                    canAttack = false;
                    break;
                }
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
        if (_bossStateManager.GetBossState() == _bossStateManager.ChaseState)
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

        if (_bossStateManager.GetBossState() == _bossStateManager.AttackState)
        {
            if(_dotProForRotation>0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
            }
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
        _front = _target.GetChild(3).GetChild(0);
        _back = _target.GetChild(3).GetChild(1);

        if (_dotPro < 0) _isFront = true;
        else _isFront = false;

        return (_dotPro < 0) ? _front.position : _back.position;
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

    private void CalculateDotProForRotation()
    {
        _directionDot = transform.position - _target.position;
        _directionDot.Normalize();

        _dotProForRotation = Vector2.Dot(_directionDot, CalculateCheckVectoForRotation());
    }

    private Vector2 CalculateCheckVectoForRotation()
    {
        return _checkVector = new Vector2(_bossStateManager.Mutant.localScale.x, .0f);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
