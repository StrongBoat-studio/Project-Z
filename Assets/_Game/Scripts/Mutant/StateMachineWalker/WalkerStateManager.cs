using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class WalkerStateManager : MonoBehaviour
{
    //State Machine variables
    WalkerBaseState currentState;
    public WalkerPatrollingState PatrollingState = new WalkerPatrollingState();
    public WalkerBeforeChaseState BeforeChaseState=new WalkerBeforeChaseState();
    public WalkerChaseState ChaseState=new WalkerChaseState();
    public WalkerAttackState AttackState=new WalkerAttackState();
    public WalkerDeathState DeathState=new WalkerDeathState();
    public WalkerHearingState HearingState = new WalkerHearingState();

    //Components use for movement
    private GameObject _mutant;
    [SerializeField] private WalkerPatrolling _walkerPatrolling;
    private GameObject _player;
    private Movement _movement; 

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)][SerializeField] private float _speed;
    [Range(0f, 10f)][SerializeField] private float _distanceChaseFront, _distanceChaseBack, _distanceAttack;
    [Range(0f, 10f)][SerializeField] private float _secondOfSleepFront, _secondOfSleepBack;
    [Range(0f, 10f)][SerializeField] private float _distanceLineOfHearing, _secondHearingNormally, _secondHearingCrounching;

    //Stealth Check for Designers
    [Header("Stealth Check")]
    [SerializeField] private float _secondsElapsed;
    [SerializeField] private float _secondsHearingElapsed;
    [SerializeField] private float _distance;
    [SerializeField] private float dotPro;
    private Transform _target;
    [SerializeField] private float _distanceChase;

    //DotPro
    private Vector2 _checkVector = new Vector2(1f, 0f);
    private Vector2 _playerPosition;
    private Vector2 _walkerPosition;
    private Vector2 _direction;

    //Variables for chasing
    private bool _zone1 = false, _zone2 = false, _zone3 = false;
    private Vector3 _target2;

    //getter and setter
    public float Speed { get { return _speed; } }
    public bool Zone1 { get { return _zone1; } set { _zone1 = value; } }
    public bool Zone2 { get { return _zone2; } set { _zone2 = value; } }
    public bool Zone3 { get { return _zone3; } set { _zone3 = value; } }
    public Vector2 CheckVector { get { return _checkVector; } set { _checkVector = value; } }
    public GameObject Mutant { get { return _mutant; } set { _mutant = value; } }
    public Vector2 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
    public Vector2 WalkerPosition { get { return _walkerPosition; } set { _walkerPosition = value; } }
    public Vector2 Direction { get { return _direction; } set { _direction = value; } }
    public float DotPro { get { return dotPro; } set { dotPro = value; } }
    public float SecondsElapsed { get { return _secondsElapsed; } set { _secondsElapsed=value; } }
    public float SecondOfSleepFront { get { return _secondOfSleepFront; } set { _secondOfSleepFront=value; } }
    public float SecondOfSleepBack { get { return _secondOfSleepBack; } set { _secondOfSleepBack=value; } }
    public float DistanceChase { get { return _distanceChase; } set { _distanceChase=value; } }
    public float DistanceChaseFront { get { return _distanceChaseFront; } set { _distanceChaseFront=value; } }
    public float DistanceChaseBack { get { return _distanceChaseBack; } set { _distanceChaseBack = value; } }
    public float Distance { get { return _distance; } set { _distance= value; } }   
    public Movement Movement { get { return _movement; } }
    public float DistanceLineOfHearing { get { return _distanceLineOfHearing; } }
    public float SecondsHearingElapsed { get { return _secondsHearingElapsed; } set { _secondsHearingElapsed=value; } }
    public float SecondHearingCrounching { get { return _secondHearingCrounching; } }
    public float SecondHearingNormally { get { return _secondHearingNormally; } }
    public GameObject Alert { get { return _alert; } }
    public Transform Target { get { return _target; } }
    public float DistanceAttack { get { return _distanceAttack; } set { _distanceAttack = value;} }
    public Animator Animator { get { return _animator; } }
    public GameObject Player { get { return _player; } }


    //Life 
    private int _mLife=100;

    //Animations
    [SerializeField] private GameObject _alert;
    [SerializeField] private Animator _animator;


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _movement= GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        _mutant = GameObject.FindGameObjectWithTag("Enemy");

        currentState = PatrollingState;
        currentState.Context = this;
        currentState.EnterState(this);

        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
    }

    void Update()
    {
        currentState.UpdateState(this);

        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _checkVector = _walkerPatrolling.GetCheckVector();
    }

    public void SwitchState (WalkerBaseState state)
    {
        currentState = state;
        currentState.Context = this; 
        state.EnterState(this);
    }

    public WalkerBaseState GetWalkerState()
    {
        return currentState;
    }

    public void TakeDamage(int damage, int weapon)
    {
        _mLife -= damage;
        if(_mLife<=0)
        {
            _mutant.SetActive(false);
            _alert.SetActive(false);
        }

        switch(weapon)
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

        Chasing();
    }

    private void ReactionToShoot()
    {
        if(_player.transform.localScale.x==1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x - .7f, _mutant.transform.position.y, _mutant.transform.position.z);

        if (_player.transform.localScale.x == -1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x + .7f, _mutant.transform.position.y, _mutant.transform.position.z);
    }

    private void ReactionToKnife()
    {
        if (_player.transform.localScale.x == 1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x - .5f, _mutant.transform.position.y, _mutant.transform.position.z);

        if (_player.transform.localScale.x == -1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x + .5f, _mutant.transform.position.y, _mutant.transform.position.z);
    }

    private void ReactionToEmpytHand()
    {
        if (_player.transform.localScale.x == 1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x - .2f, _mutant.transform.position.y, _mutant.transform.position.z);

        if (_player.transform.localScale.x == -1)
            _mutant.transform.position = new Vector3(_mutant.transform.position.x + .2f, _mutant.transform.position.y, _mutant.transform.position.z);
    }

    private void Chasing()
    {
        _target2 = new Vector3(_target.position.x, transform.position.y, transform.position.z);

        transform.position = Vector2.MoveTowards(transform.position, _target2, (_speed + 1f) * Time.deltaTime);
    }
}
