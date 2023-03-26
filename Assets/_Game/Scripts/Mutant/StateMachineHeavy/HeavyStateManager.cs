using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeavyStateManager : MonoBehaviour
{
    //State Machine variables
    HeavyBaseState currentState;
    public HeavyPoToPoState PoToPoState = new HeavyPoToPoState();
    public HeavyChaseState ChaseState = new HeavyChaseState();
    public HeavyAttackState AttackState = new HeavyAttackState();
    public HeavyBeforeChaseState BeforeChaseState = new HeavyBeforeChaseState();
    public HeavyDeathState DeathState = new HeavyDeathState();

    //Movement variables
    [SerializeField] private Vector3 _pos1, _pos2, _startPos;
    [SerializeField] private GameObject _mutant, _player;
    private Vector3 nextPos;

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)][SerializeField] private float _speed;
    [Range(0f, 10f)][SerializeField] private float _distanceChase, _distanceAttack;
    [Range(0f, 10f)][SerializeField] private float _secondOfSleep;

    //Stealth Chack for Designers
    [Header("Stealth Chack")]
    [SerializeField] private float _secondsElapsed;
    [SerializeField] private float _distance;

    //Variables for chasing
    private Transform _target;
    private bool _targetCheck = false;
    private bool _chase = false;
    private bool _zone1 = false, _zone2 = false, _zone3 = false;

    //getter and setter
    public Vector3 Pos1 { get { return _pos1; } }
    public Vector3 Pos2 { get { return _pos2; } }
    public Vector3 NextPos { get { return nextPos; } set { nextPos = value; } }
    public float Speed { get { return _speed; } }
    public bool Chase { get { return _chase; } set { _chase = value; } }
    public bool Zone1 { get { return _zone1; } set { _zone1 = value; } }
    public bool Zone2 { get { return _zone2; } set { _zone2 = value; } }
    public bool Zone3 { get { return _zone3; } set { _zone3 = value; } }
    public float SecondOfSleep { get { return _secondOfSleep; } set { _secondOfSleep = value; } }
    public float SecondsElapsed { get { return _secondsElapsed; } set { _secondsElapsed = value; } }
    public float Distance { get { return _distance; } }
    public float DistanceChase { get { return _distanceChase; } }
    public Transform Target { get { return _target; } set { _target = value; } }
    public bool TargetCheck { get { return _targetCheck; } set { _targetCheck = value; } }
    public float DistanceAttack { get { return _distanceAttack; } }
    public GameObject Alert { get { return _alert; } }
    public int Life { get { return _life; } set { _life = value; } }
    public int MLife { get { return _mLife; } set { _mLife = value; } }
    public GameObject M {get { return _m; }}

    //UI
    [Header("UI Variables")]
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _lifeText;
    private int _life=100;
    [SerializeField] private TextMeshProUGUI _mLifeText;
    private int _mLife = 100;
    [SerializeField] private GameObject _m;

    //Animations
    [SerializeField] private GameObject _alert;

    void Awake()
    {
        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Start()
    {
        currentState = PoToPoState;
        currentState.Context = this;
        currentState.EnterState(this);

        nextPos = _startPos;
        _secondsElapsed = _secondOfSleep;

    }

    void Update()
    {
        currentState.UpdateState(this);
        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        _time.text = "Time: " + Mathf.Clamp(Mathf.CeilToInt(_secondsElapsed), 0, float.MaxValue).ToString();
        _lifeText.text = "Player life: " + Mathf.Clamp(_life, 0, 100).ToString();
        _mLifeText.text = "Player life: " + Mathf.Clamp(_mLife, 0, 100).ToString();

        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if(_life==0)
        {
            SceneManager.LoadScene(1);
        }

        if (_mLife == 0)
        {
            SwitchState(DeathState);
            SceneManager.LoadScene(2);
        }
    }

    public void SwitchState(HeavyBaseState state)
    {
        currentState = state;
        currentState.Context = this;
        state.EnterState(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_pos1, _pos2);
    }
}
