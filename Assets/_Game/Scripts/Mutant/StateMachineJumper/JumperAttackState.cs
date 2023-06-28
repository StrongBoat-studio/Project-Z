using UnityEngine;

public class JumperAttackState : JumperBaseState 
{
    private float _time = 1f;
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Attack State");
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        if (Context.Distace > Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.PatrollingState);
        }
    }
}
