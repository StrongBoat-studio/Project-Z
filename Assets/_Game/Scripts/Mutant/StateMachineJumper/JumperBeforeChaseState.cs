using UnityEngine;

public class JumperBeforeChaseState : JumperBaseState 
{
    public override void EnterState(JumperStateManager jumper)
    {
        
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        TimeManipulation();
        PositionCheck();
        TimeSight();

        if (Context.Distace>Context.DistanceSight)
        {
            jumper.SwitchState(jumper.HearingState); 
        }

        if (Context.SecondsElapsedHearing<=0 || Context.SecondsElapsedSight<=0)
        {
            jumper.SwitchState(jumper.ChaseState);
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

        if (Context.Distace <= 6 && !Context.Zone2)
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

    private void TimeSight()
    {
        if(Context.DotPro>0)
        {
            Context.SecondsElapsedSight -= Time.deltaTime;
        }
        else
        {
            Context.SecondsElapsedSight = Context.SecondsSight;
        }
    }
}
