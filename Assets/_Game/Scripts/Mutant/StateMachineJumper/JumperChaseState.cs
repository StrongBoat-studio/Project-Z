using UnityEngine;

public class JumperChaseState : JumperBaseState
{
    private Vector3 _target;
    public override void EnterState(JumperStateManager jumper)
    {

    }

    public override void UpdateState(JumperStateManager jumper)
    {
        PositionCheck();

        if (Context.Distace>Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.PatrollingState);
        }

        if(Context.Distace<=Context.DistanceAttack)
        {
            jumper.SwitchState(jumper.AttackState);
        }
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        if (Context.Player == null)
        {
            return;
        }

        Context.PlayerPosition = Context.Player.position;
        Context.JumperPosition = Context.Mutant.position;

        Context.Direction = Context.PlayerPosition - Context.JumperPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
