using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HeavyChaseState : HeavyBaseState
{
    public override void EnterState(HeavyStateManager heavy)
    {
        Debug.Log("Chase state o³ jea");
    }

    public override void UpdateState(HeavyStateManager heavy)
    {
        Chasing();

        if (Context.Distance > Context.DistanceChase)
        {
            heavy.SwitchState(heavy.PoToPoState);
        }

        if(Context.Distance < Context.DistanceAttack) 
        {
            heavy.SwitchState(heavy.AttackState);
        }
    }

    private void Chasing()
    {
        Context.transform.position = Vector2.MoveTowards(Context.transform.position, Context.Target.position, (Context.Speed + 1f) * Time.deltaTime);
    }
}
