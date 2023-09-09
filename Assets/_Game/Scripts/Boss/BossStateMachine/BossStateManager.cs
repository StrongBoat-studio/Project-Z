using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : MonoBehaviour
{

    BossBaseState currentState;
    public BossWaitingState WaitingState = new BossWaitingState();
    public BossChaseState ChaseState = new BossChaseState();
    public BossAttackState AttackState = new BossAttackState();
    public BossDeathState DeathState = new BossDeathState();

    //Stealth Settings for Designers
    [Header("Stealth Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed = 2.0f;
    [Range(0f, 10f)] [SerializeField] private float _distanceChase = 5.0f, _distanceAttack = 2.3f;
    [SerializeField] private float _distance;


    private Transform _player;
    private Transform _mutant;
    private Animator _animator;

    //Life 
    [SerializeField] private int _mLife = 100;

    //getter and setter
    public float Speed { get { return _speed; } }
    public float DistanceChase { get { return _distanceChase; } }
    public float DistanceAttack { get { return _distanceAttack; } }
    public Animator Animator { get { return _animator; } }
    public Transform Mutant { get { return _mutant; } }

    private void Awake()
    {
        _player = GameManager.Instance.player;
        _mutant = this.gameObject.transform;
        _animator = GetComponentInChildren<Animator>();
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
    }

}
