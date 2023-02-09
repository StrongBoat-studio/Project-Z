using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HeavyPoToPoState : HeavyBaseState 
{
    public override void EnterState (HeavyStateManager heavy)
    {
        Debug.Log("Point to point");

        FalseBool();
        Context.Alert.SetActive(false);
    }

    public override void UpdateState(HeavyStateManager heavy)
    {
        Moving();

        if (Context.Distance<=Context.DistanceChase)
        {
            heavy.SwitchState(heavy.BeforeChaseState);
        }
    }


    private void Moving()
    {
        if (Context.transform.position == Context.Pos1)
        {
            Context.NextPos = Context.Pos2;
        }

        if (Context.transform.position == Context.Pos2)
        {
            Context.NextPos = Context.Pos1;
        }
        Context.transform.position = Vector3.MoveTowards(Context.transform.position, Context.NextPos, Context.Speed * Time.deltaTime);
    }

    private void FalseBool()
    {
        Context.Chase = false;
        Context.Zone1 = false;
        Context.Zone2 = false;
        Context.Zone3 = false;
        Context.SecondsElapsed = Context.SecondOfSleep;
    }

}
