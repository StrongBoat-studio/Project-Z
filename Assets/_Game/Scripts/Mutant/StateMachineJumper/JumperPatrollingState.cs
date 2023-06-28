using UnityEngine;

public class JumperPatrollingState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Patrolling State");

        Context.Zone1 = false;
        Context.Zone2 = false;

        Context.SecondsElapsedHearing = Context.SecondsHearing;
        Context.Alert.SetActive(false);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        PositionCheck();

        if (Context.Distace<=Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.HearingState);
        }
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        Context.PlayerPosition = Context.Player.position;
        Context.JumperPosition = Context.Mutant.position;

        Context.Direction = Context.PlayerPosition - Context.JumperPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
