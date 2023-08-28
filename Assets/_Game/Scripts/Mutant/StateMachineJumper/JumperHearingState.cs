using UnityEngine;

public class JumperHearingState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {


        Context.Alert.SetActive(true);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        TimeManipulation();
        PositionCheck();

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

        if (Context.Distace <= Context.DistanceAttack)
        {
            jumper.SwitchState(jumper.AttackState);
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

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        if (Context.Player == null)
        {
            return;
        }

        Context.PlayerPosition = Context.Player.position;
        Context.JumperPosition = Context.Mutant.position;

        Context.Direction = Context.PlayerPosition - Context.JumperPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
