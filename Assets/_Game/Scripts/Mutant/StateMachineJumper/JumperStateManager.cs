using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumperStateManager : MonoBehaviour, IMutantInit
{
    //State Machine variables
    JumperBaseState currentState;
    public JumperPatrollingState PatrollingState =new JumperPatrollingState();
    public JumperHearingState HearingState=new JumperHearingState();
    public JumperBeforeChaseState BeforeChaseState=new JumperBeforeChaseState();
    public JumperChaseState ChaseState=new JumperChaseState();
    public JumperAttackState AttackState=new JumperAttackState();
    public JumperDeathState DeathState=new JumperDeathState();

    //Movement variables
    private Transform _mutant;
    private Transform _player;
    private JumperPatrolling _jumperPatrolling;

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed = 3.0f;
    [Range(0f, 10f)] [SerializeField] private float _distanceAttack = 1.5f;
    [Range(0f, 10f)] [SerializeField] private float _distanceHearing = 10.0f;
    [Range(0f, 10f)] [SerializeField] private float _secondsHearing = 6.0f;
    [Range(0f, 10f)] [SerializeField] private float _distanceSight = 6.0f;
    [Range(0f, 10f)] [SerializeField] private float _secondsSight = 2.0f;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _sliderObject;

    //Variables for chasing
    private bool _zone1 = false;
    private bool _zone2 = false;
    private float _secondElapsedHearing;
    private float _secondElapsedSight;

    //Attack
    private Rigidbody2D _rigidbody2D;
    private JumperChaseWithAI _jumperChaseWithAI;

    //Stealth Chack for Designers
    [Header("Stealth Chack")]
    [SerializeField] private float _distance;
    [SerializeField] private float dotPro=0;
    [SerializeField] private string _jumperCurrentState;

    //Dot
    private Vector2 _checkVector = new Vector2(1f, 0f);
    private Vector2 _playerPosition;
    private Vector2 _jumperPosition;
    private Vector2 _direction;

    //getter and setter
    public float Speed { get { return _speed; } set { _speed = value; } }
    public Vector2 CheckVector { get { return _checkVector; } set { _checkVector= value; } }
    public Transform Mutant { get { return _mutant; } }
    public float Distace { get { return _distance; } }
    public float DistanceHearing { get { return _distanceHearing; } }
    public bool Zone1 { get { return _zone1; } set { _zone1 = value; } }
    public bool Zone2 { get { return _zone2; } set { _zone2 = value; } }
    public float SecondsHearing { get { return _secondsHearing; } }
    public float SecondsElapsedHearing { get { return _secondElapsedHearing; } set { _secondElapsedHearing=value; } }
    public float DistanceSight { get { return _distanceSight; } set { _distanceSight = value; } }
    public float SecondsSight { get { return _secondsSight; } set { _secondsSight = value; } }
    public Vector2 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
    public Vector2 JumperPosition { get { return _jumperPosition; } set { _jumperPosition = value; } }
    public Vector2 Direction { get { return _direction; } set { _direction = value; } } 
    public float DotPro { get { return dotPro; } set { dotPro = value; } }
    public float SecondsElapsedSight { get { return _secondElapsedSight; } set { _secondElapsedSight = value; } }
    public GameObject Alert { get { return _alert; } }
    public float DistanceAttack { get { return _distanceAttack; } }
    public Transform Player { get { return _player; } }
    public Animator Animator { get { return _animator; } }

    //UI
    private int _mLife = 100;

    //Animations
    [SerializeField] private GameObject _alert;
    private Animator _animator;

    public bool canAttack = true;

    [SerializeField] string _stateDebug;

    private void Awake()
    {
        _player = GameManager.Instance.player;
        _jumperPatrolling = GetComponent<JumperPatrolling>();
        _mutant = transform;
        _animator = GetComponentInChildren<Animator>();
        _jumperChaseWithAI = GetComponent<JumperChaseWithAI>();

        currentState = PatrollingState;
        currentState.Context = this;
        currentState.EnterState(this);

        if (_player != null)
        {
            _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        }
        else
        {
            _distance = Mathf.Infinity;
        }

        _secondElapsedHearing = _secondsHearing;
        _secondElapsedSight = _secondsSight;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        if(transform.position.y>2.2f)
        {
            transform.position = new Vector2(transform.position.x, -0.273f);
        }

        if (_player != null)
        {
            _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        }
        else
        {
            _distance = Mathf.Infinity;
            _player = GameManager.Instance.player;
        }

        _checkVector = _jumperPatrolling.GetCheckVector();
        _mutant = transform;

        if (currentState == PatrollingState) _stateDebug = "Patrollling";
        if (currentState == HearingState) _stateDebug = "Hearing";
        if (currentState == ChaseState) _stateDebug = "Chase";
        if (currentState == BeforeChaseState) _stateDebug = "BeforeChase";
        if (currentState == AttackState) _stateDebug = "Attack";
    }

    public void SwitchState(JumperBaseState state)
    {
        currentState = state;
        currentState.Context = this;
        state.EnterState(this);
    }

    public void SwitchStateToAttack()
    {
        currentState = AttackState;
    }

    public JumperBaseState GetJumperState()
    {
        return currentState;
    }

    public void Jump()
    {
        _jumperChaseWithAI.Jump();
    }

    public void TakeDamage(int damage, int weapon)
    {
        _mLife -= damage;
        _slider.value -= damage * 0.01f;
        if (_mLife <= 0)
        {
            currentState = DeathState;
            _sliderObject.SetActive(false);

            _animator.SetBool("IsStanding", false);
            _animator.SetBool("IsAttack1", false);
            _animator.SetBool("IsAttack2", false);
            _animator.SetBool("IsDeath", true);

            _alert.SetActive(false);
        }

        switch (weapon)
        {
            case 0:
                {
                    ReactionToEmpytHand();
                    break;
                }
            case 1:
                {
                    ReactionToShoot();
                    break;
                }
            case 2:
                {
                    ReactionToKnife();
                    break;
                }
        }
    }

    private void ReactionToShoot()
    {
        if(_player==null)
        {
            return;
        }

        if (_player.transform.localScale.x == 1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x - .7f, _mutant.transform.position.y, _mutant.transform.position.z);

        if (_player.transform.localScale.x == -1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x + .7f, _mutant.transform.position.y, _mutant.transform.position.z);
    }

    private void ReactionToKnife()
    {
        if (_player == null)
        {
            return;
        }

        if (_player.transform.localScale.x == 1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x - .5f, _mutant.transform.position.y, _mutant.transform.position.z);

        if (_player.transform.localScale.x == -1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x + .5f, _mutant.transform.position.y, _mutant.transform.position.z);
    }

    private void ReactionToEmpytHand()
    {
        if (_player == null)
        {
            return;
        }

        if (_player.transform.localScale.x == 1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x - .2f, _mutant.transform.position.y, _mutant.transform.position.z);

        if (_player.transform.localScale.x == -1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x + .2f, _mutant.transform.position.y, _mutant.transform.position.z);
    }

    public void DestroyJumper()
    {
        //SaveSystem Code

        Destroy(this.gameObject);
    }

    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    public int GetLife()
    {
        return _mLife;
    }

    public void SetLife(int life)
    {
        _mLife = life;
    }
    
    public Vector3 GetPosition()
    {
        return _mutant.position;
    }

    public void SetPosition(Vector3 position)
    {
        _mutant.position = position;
    }

    public void SetDefaultState()
    {
        currentState = PatrollingState;
    }

    public LevelManagerData.MutantType GetMutantType()
    {
        return LevelManagerData.MutantType.Jumper;
    }

    public void UpdateInit()
    {
        _slider.value = _mLife / 100f;
    }
}
