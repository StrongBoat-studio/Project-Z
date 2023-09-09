using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaitingState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss waiting state");
        Context.Animator.SetBool("IsWalking", false);

        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantIdle, Context.Mutant.position);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if(Context.DistanceChase > Context.GetDistance())
        {
            boss.SwitchState(boss.ChaseState);
        }
    }
}
