using Unity.VisualScripting;
using UnityEngine;

public class HeavyBeforeChaseState : HeavyBaseState
{
    public override void EnterState(HeavyStateManager heavy)
    {
        Debug.Log("Before Chase State");

        Context.Chase = true;
        Context.Alert.SetActive(true);
    }

    public override void UpdateState(HeavyStateManager heavy)
    {
        Moving();
        TimeManipulation();

        if (Context.Distance > Context.DistanceChase)
        {
            heavy.SwitchState(heavy.PoToPoState);
        }

        if (Context.SecondsElapsed<=0)
        {
            heavy.SwitchState(heavy.ChaseState);
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

    private void TimeManipulation()
    {
        Context.SecondsElapsed -= Time.deltaTime;

        if (Context.Distance <= 5 && !Context.Zone1)
        {
            Debug.Log("-2");
            Context.SecondsElapsed -= 2;
            Context.Zone1 = true;
        }

        if (Context.Distance <= 4 && !Context.Zone2)
        {
            Debug.Log("-1");
            Context.SecondsElapsed -= 1;
            Context.Zone2 = true;
        }

        if (Context.Distance <= 3 && !Context.Zone2!)
        {
            Debug.Log("0");
            Context.SecondsElapsed = 0;
            Context.Zone3 = true;
        }
    }
}
