using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WalkerChaseState : WalkerBaseState
{
    private Vector3 _target;

    public override void EnterState(WalkerStateManager walker)
    {

    }

    public override void UpdateState(WalkerStateManager walker)
    {
        PositionCheck();

        if(Context.Distance>Context.DistanceChase)
        {
            walker.SwitchState(walker.PatrollingState);
        }

        if (Context.Distance<Context.DistanceAttack)
        {
            walker.SwitchState(walker.AttackState);
        }
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        if (Context.Player == null)
        {
            return;
        }

        Context.PlayerPosition = Context.Player.transform.position;
        Context.WalkerPosition = Context.Mutant.transform.position;

        Context.Direction = Context.PlayerPosition - Context.WalkerPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
