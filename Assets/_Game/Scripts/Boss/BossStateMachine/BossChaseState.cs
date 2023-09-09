using UnityEngine;

public class BossChaseState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss chase state");
        Context.Animator.SetBool("IsWalking", true);

        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantGrowl, Context.Mutant.position);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (Context.DistanceChase < Context.GetDistance())
        {
            boss.SwitchState(boss.WaitingState);
        }

        if(Context.DistanceAttack >= Context.GetDistance())
        {
            boss.SwitchState(boss.AttackState);
        }
    }
}
