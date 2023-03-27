using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WalkerChaseState : WalkerBaseState
{
    private Vector3 _target;

    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Chase State");
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        Chasing();
        PositionCheck();

        if(Context.Distance>Context.DistanceChase)
        {
            walker.SwitchState(walker.PoToPoState);
        }

        if (Context.Distance<Context.DistanceAttack)
        {
            walker.SwitchState(walker.AttackState);
        }
    }

    private void Chasing()
    {
        _target = new Vector3(Context.Target.position.x, Context.transform.position.y, Context.transform.position.z);

        Context.transform.position = Vector2.MoveTowards(Context.transform.position, _target, (Context.Speed + 1f) * Time.deltaTime);
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        Context.PlayerPosition = Context.Player2.position;
        Context.WalkerPosition = Context.Walker.position;

        Context.Direction = Context.PlayerPosition - Context.WalkerPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
