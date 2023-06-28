using UnityEngine;

public class JumperPatrollingState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Point To Point State");

        Context.Zone1 = false;
        Context.Zone2 = false;

        Context.SecondsElapsedHearing = Context.SecondsHearing;
        Context.Alert.SetActive(false);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        if(Context.Distace<=Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.HearingState);
        }
    }
}
