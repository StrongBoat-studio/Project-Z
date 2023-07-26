using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WalkerStateManager : MonoBehaviour, IMutantInit
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
    private Transform _mutant;
    private WalkerPatrolling _walkerPatrolling;
    private Transform _player;
    private Movement _movement; 

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)][SerializeField] private float _speed=2.0f;
    [Range(0f, 10f)][SerializeField] private float _distanceChaseFront=4.0f, _distanceChaseBack=2.0f, _distanceAttack=1.0f;
    [Range(0f, 10f)][SerializeField] private float _secondOfSleepFront=6.0f, _secondOfSleepBack=2.0f;
    [Range(0f, 10f)][SerializeField] private float _distanceLineOfHearing=6.0f, _secondHearingNormally=3.0f, _secondHearingCrounching=10.0f;

    //Stealth Check for Designers
    [Header("Stealth Check")]
    [SerializeField] private float _secondsElapsed;
    [SerializeField] private float _secondsHearingElapsed;
    [SerializeField] private float _distance;
    [SerializeField] private float dotPro;
    [SerializeField] private float _distanceChase;

    [Header("Health")]
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _sliderObject;

    //DotPro
    private Vector2 _checkVector = new Vector2(1f, 0f);
    private Vector2 _playerPosition;
    private Vector2 _walkerPosition;
    private Vector2 _direction;

    //Variables for chasing
    private bool _zone1 = false, _zone2 = false, _zone3 = false;

    //getter and setter
    public float Speed { get { return _speed; } }
    public bool Zone1 { get { return _zone1; } set { _zone1 = value; } }
    public bool Zone2 { get { return _zone2; } set { _zone2 = value; } }
    public bool Zone3 { get { return _zone3; } set { _zone3 = value; } }
    public Vector2 CheckVector { get { return _checkVector; } set { _checkVector = value; } }
    public Transform Mutant { get { return _mutant; } set { _mutant = value; } }
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
    public float DistanceAttack { get { return _distanceAttack; } set { _distanceAttack = value;} }
    public Animator Animator { get { return _animator; } }
    public Transform Player { get { return _player; } }

    //Life 
    [SerializeField] private int _mLife=100;

    //Animations
    [SerializeField] private GameObject _alert;
    private Animator _animator;
    public bool isPickUp = false;

    void Awake()
    {
        _player = GameManager.Instance.player;
        _movement = GameManager.Instance.movement;
        _mutant = this.gameObject.transform;
        _walkerPatrolling = GetComponent<WalkerPatrolling>();
        _animator = GetComponentInChildren<Animator>();
        

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

        _slider.value = 1f;
    }

    void Update()
    {
        currentState.UpdateState(this);

        if(_player != null)
        {
            _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        }
        else
        {
            _distance = Mathf.Infinity;
            _player = GameManager.Instance.player;
        }

        _checkVector = _walkerPatrolling.GetCheckVector();

        _mutant = transform;
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
        _slider.value -= damage * 0.01f;
        if(_mLife<=0)
        {
            currentState = DeathState;
            _sliderObject.SetActive(false);
            _animator.SetBool("IsDeath", true);
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
    }

    private void ReactionToShoot()
    {
        if(_player==null)
        {
            return;
        }

        if(_player.transform.localScale.x==1)
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

    public void DestroyWalker()
    {
        //SaveSystem Code

        GetComponent<WalkerPatrolling>().enabled = true;
        GetComponent<WalkerChaseWithAI>().enabled = true;
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
        _mutant.transform.position = position;
    }

    public void SetDefaultState()
    {
        currentState = PatrollingState;
    }

    public LevelManagerData.MutantType GetMutantType()
    {
        return LevelManagerData.MutantType.Walker;
    }

    public void UpdateInit()
    {
        _slider.value = _mLife / 100f;
    }
}
