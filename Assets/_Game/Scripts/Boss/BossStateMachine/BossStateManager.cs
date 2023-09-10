using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStateManager : MonoBehaviour
{

    BossBaseState currentState;
    public BossWaitingState WaitingState = new BossWaitingState();
    public BossChaseState ChaseState = new BossChaseState();
    public BossAttackState AttackState = new BossAttackState();
    public BossDeathState DeathState = new BossDeathState();
    public BossQTEState QTEState = new BossQTEState();

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed = 2.0f;
    [Range(0f, 10f)] [SerializeField] private float _distanceChase = 5.0f, _distanceAttack = 2.3f;
    [SerializeField] private float _distance;


    private Transform _player;
    private Transform _mutant;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;

    //Life 
    [SerializeField] private int _mLife = 100;

    [Header("Health")]
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _sliderObject;
    [SerializeField] private GameObject _alert;

    //Attack
    public int nextAttackId = 1;
    private float _addFront = 0;
    private float _addBack = 0;

    //getter and setter
    public float Speed { get { return _speed; } }
    public float DistanceChase { get { return _distanceChase; } }
    public float DistanceAttack { get { return _distanceAttack; } }
    public Animator Animator { get { return _animator; } }
    public Transform Mutant { get { return _mutant; } }
    public MonoBehaviour MonoBehaviour;
    public float AddFront { get { return _addFront; } set { _addFront = value; } }
    public float AddBack { get { return _addBack; } set { _addBack = value; } }
    public BoxCollider2D BoxCollider2D { get { return _boxCollider2D; } }

    private void Awake()
    {
        _player = GameManager.Instance.player;
        _mutant = this.gameObject.transform;
        _animator = GetComponentInChildren<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        MonoBehaviour = GetComponent<MonoBehaviour>();
    }

    void Start()
    {
        currentState = WaitingState;
        currentState.Context = this;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);

        if (_player != null)
        {
            _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);
        }
        else
        {
            _distance = Mathf.Infinity;
            _player = GameManager.Instance.player;
        }
    }

    public void SwitchState(BossBaseState state)
    {
        currentState = state;
        currentState.Context = this;
        state.EnterState(this);
    }

    public float GetDistance()
    {
        return _distance;
    }

    public BossBaseState GetBossState()
    {
        return currentState;
    }

    public int GetLife()
    {
        return _mLife;
    }

    public void TakeDamage(int damage, int weapon)
    {
        _mLife -= damage;
        _slider.value -= damage * 0.01f;

        if (_mLife <= 0)
        {
            SwitchState(DeathState);
            _sliderObject.SetActive(false);
            //_animator.SetBool("IsDeath", true);
            //_alert.SetActive(false);
        }
    }

    public void SetPlayerChaseAPoints()
    {
        //Reset Points
        _player.GetChild(3).GetChild(0).localPosition = new Vector2(-0.7f, _player.GetChild(3).GetChild(0).localPosition.y);
        _player.GetChild(3).GetChild(1).localPosition = new Vector2(0.8f, _player.GetChild(3).GetChild(1).localPosition.y);

        //Front
        _player.GetChild(3).GetChild(0).localPosition = new Vector2(_addFront, _player.GetChild(3).GetChild(0).localPosition.y);

        //Back
        _player.GetChild(3).GetChild(1).localPosition = new Vector2(_addBack, _player.GetChild(3).GetChild(1).localPosition.y);
    }

    public void SetChaseSpeed(float speed)
    {
        GetComponent<BossChaseWithAI>().SetSpeed(speed);
    }

}
