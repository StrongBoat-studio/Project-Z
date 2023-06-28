using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumperStateManager : MonoBehaviour
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
    [SerializeField] private GameObject _mutant;
    private GameObject _player;
    [SerializeField] private Transform _jumper;
    private Transform _player2;
    private Vector3 nextPos;

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)][SerializeField] private float _speed;
    [Range(0f, 10f)][SerializeField] private float _distanceAttack;
    [Range(0f, 10f)][SerializeField] private float _distanceHearing;
    [Range(0f, 10f)][SerializeField] private float _secondsHearing;
    [Range(0f, 10f)][SerializeField] private float _distanceSight;
    [Range(0f, 10f)][SerializeField] private float _secondsSight;
    [SerializeField] private Slider _slider;


    //Variables for chasing
    private Vector2 _checkVector;
    private bool _zone1 = false;
    private bool _zone2 = false;
    private float _secondElapsedHearing;
    private float _secondElapsedSight;

    //Stealth Chack for Designers
    [Header("Stealth Chack")]
    [SerializeField] private float _distance;
    private Transform _target;
    [SerializeField] private float dotPro=0;

    //Dot
    private Vector2 _playerPosition;
    private Vector2 _jumperPosition;
    private Vector2 _direction;

    //getter and setter
    public Vector3 NextPos { get { return nextPos; } set { nextPos = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public Vector2 CheckVector { get { return _checkVector; } set { _checkVector= value; } }
    public GameObject Mutant { get { return _mutant; } }
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
    public Transform Player2 { get { return _player2; } }
    public Transform Jumper { get { return _jumper; } }
    public Vector2 Direction { get { return _direction; } set { _direction = value; } } 
    public float DotPro { get { return dotPro; } set { dotPro = value; } }
    public float SecondsElapsedSight { get { return _secondElapsedSight; } set { _secondElapsedSight = value; } }
    public GameObject Alert { get { return _alert; } }
    public float DistanceAttack { get { return _distanceAttack; } }
    public Transform Target { get { return _target; } }

    //UI
    private int _mLife = 100;

    //Animations
    [SerializeField] private GameObject _alert;
    private Animator _animator;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _player2 = GameManager.Instance.player;


        currentState = PatrollingState;
        currentState.Context = this;
        currentState.EnterState(this);


        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);

        _secondElapsedHearing = _secondsHearing;
        _secondElapsedSight = _secondsSight;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void SwitchState(JumperBaseState state)
    {
        currentState = state;
        currentState.Context = this;
        state.EnterState(this);
    }

    public JumperBaseState GetJumperState()
    {
        return currentState;
    }

    public void TakeDamage(int damage, int weapon)
    {
        _mLife -= damage;
        _slider.value -= damage * 0.01f;
        if (_mLife <= 0)
        {
            _mutant.SetActive(false);
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
        if (_player.transform.localScale.x == 1)
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
}
