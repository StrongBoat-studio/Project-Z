using UnityEngine;

public class BossChaseState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss chase state");
        Context.Animator.SetBool("IsWalking", true);

        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantGrowl, Context.Mutant.position);
        Context.Animator.SetBool("IsAttacking1", true);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (Context.DistanceChase < Context.GetDistance())
        {
            boss.SwitchState(boss.WaitingState);
        }
    }
}
