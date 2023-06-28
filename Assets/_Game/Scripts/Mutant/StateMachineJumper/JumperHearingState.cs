using UnityEngine;

public class JumperHearingState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Hearing State");

        Context.Alert.SetActive(true);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        TimeManipulation();

        if (Context.Distace > Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.PatrollingState);
        }

        if (Context.SecondsElapsedHearing<=0)
        {
            jumper.SwitchState(jumper.ChaseState);
        }

        if (Context.Distace<=Context.DistanceSight)
        {
            jumper.SwitchState(jumper.BeforeChaseState);
        }


    }

    private void TimeManipulation()
    {
        Context.SecondsElapsedHearing -= Time.deltaTime;

        if (Context.Distace <= 8 && !Context.Zone1)
        {
            Context.SecondsElapsedHearing -= 2;
            Context.Zone1 = true;
        }

        if (Context.Distace <=6  && !Context.Zone2)
        {
            Context.SecondsElapsedHearing -= 2;
            Context.Zone2 = true;
        }

    }
}
