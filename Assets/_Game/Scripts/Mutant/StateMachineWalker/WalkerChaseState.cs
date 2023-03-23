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
}
