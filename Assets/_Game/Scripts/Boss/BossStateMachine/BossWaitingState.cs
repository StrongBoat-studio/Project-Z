using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaitingState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss waiting state");
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            boss.SwitchState(boss.ChaseState);
        }
    }
}
