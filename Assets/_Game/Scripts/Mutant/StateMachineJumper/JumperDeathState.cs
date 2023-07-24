using UnityEngine;

public class JumperDeathState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {
        Context.Animator.SetBool("IsAttack1", false);
        Context.Animator.SetBool("IsAttack2", false);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        
    }
}
