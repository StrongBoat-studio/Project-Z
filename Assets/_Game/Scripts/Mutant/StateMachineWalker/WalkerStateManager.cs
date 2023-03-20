using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WalkerStateManager : MonoBehaviour
{
    //State Machine variables
    WalkerBaseState currentState;
    public WalkerPoToPoState PoToPoState=new WalkerPoToPoState();
    public WalkerBeforeChaseState BeforeChaseState=new WalkerBeforeChaseState();
    public WalkerChaseState ChaseState=new WalkerChaseState();
    public WalkerAttackState AttackState=new WalkerAttackState();
    public WalkerDeathState DeathState=new WalkerDeathState();
    public WalkerHearingState HearingState = new WalkerHearingState();

    //Movement variables
    [SerializeField] private Vector3 _pos1, _pos2, _startPos;
    [SerializeField] private GameObject _mutant, _player;
    [SerializeField] private Transform _walker, _player2;
    private Vector3 nextPos;
    [SerializeField] private Movement _movement; 

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)][SerializeField] private float _speed;
    [Range(0f, 10f)][SerializeField] private float _distanceChaseFront, _distanceChaseBack, _distanceAttack;
    [Range(0f, 10f)][SerializeField] private float _secondOfSleepFront, _secondOfSleepBack;
    [Range(0f, 10f)][SerializeField] private float _distanceLineOfHearing, _secondHearingNormally, _secondHearingCrounching;

    //Stealth Chack for Designers
    [Header("Stealth Chack")]
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

    //getter and setter
    public Vector3 Pos1 { get { return _pos1; } }
    public Vector3 Pos2 { get { return _pos2; } }
    public Vector3 NextPos { get { return nextPos; } set { nextPos = value; } }
    public float Speed { get { return _speed; } }
    public bool Zone1 { get { return _zone1; } set { _zone1 = value; } }
    public bool Zone2 { get { return _zone2; } set { _zone2 = value; } }
    public bool Zone3 { get { return _zone3; } set { _zone3 = value; } }
    public Vector2 CheckVector { get { return _checkVector; } set { _checkVector = value; } }
    public GameObject Mutant { get { return _mutant; } set { _mutant = value; } }
    public Vector2 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
    public Vector2 WalkerPosition { get { return _walkerPosition; } set { _walkerPosition = value; } }
    public Vector2 Direction { get { return _direction; } set { _direction = value; } }
    public Transform Walker { get { return _walker; } set { _walker= value; } }
    public Transform Player2 { get { return _player2; } set { _player2 = value;} }
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
    public int PLife { get { return _pLife; } set { _pLife = value; } }
    public int MLife { get { return _mLife; } set { _mLife = value; } }
    public float DistanceAttack { get { return _distanceAttack; } set { _distanceAttack = value;} }
    public GameObject M { get { return _m; } }
    public Animator Animator { get { return _animator; } }


    //Ui 
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _timeS;
    [SerializeField] private TextMeshProUGUI _timeH;
    [SerializeField] private TextMeshProUGUI _lifeP;
    [SerializeField] private TextMeshProUGUI _lifeM;
    private int _pLife=100;
    private int _mLife=100;
    [SerializeField] private GameObject _m;

    //Animations
    [SerializeField] private GameObject _alert;
    [SerializeField] private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PoToPoState;
        currentState.Context = this;
        currentState.EnterState(this);

        nextPos = _startPos;
        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _timeS.text = "Time seeing: " + Mathf.Clamp(Mathf.CeilToInt(_secondsElapsed), 0, float.MaxValue).ToString();
        _timeH.text = "Time hearing: " + Mathf.Clamp(Mathf.CeilToInt(_secondsHearingElapsed), 0, float.MaxValue).ToString();

        _lifeP.text="Player life: "+Mathf.Clamp(_pLife, 0, int.MaxValue).ToString();
        _lifeM.text = "Mutant life: " + Mathf.Clamp(_mLife, 0, int.MaxValue).ToString();

        if (_pLife == 0)
        {
            SceneManager.LoadScene(1);
        }

        if (_mLife == 0)
        {
            SwitchState(DeathState);
            SceneManager.LoadScene(2);
        }
    }

    public void SwitchState (WalkerBaseState state)
    {
        currentState = state;
        currentState.Context = this; 
        state.EnterState(this);
    }
}
