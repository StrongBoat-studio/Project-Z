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
    }

    public void SwitchState(BossBaseState state)
    {
        currentState = state;
        currentState.Context = this;
        state.EnterState(this);
    }
}
