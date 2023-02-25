using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WalkerChaseState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Chase State");
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        Chasing();

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
        Context.transform.position = Vector2.MoveTowards(Context.transform.position, Context.Target.position, (Context.Speed + 1f) * Time.deltaTime);
    }
}
