using UnityEngine;

public class JumperBeforeChaseState : JumperBaseState 
{
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Before Chase State");
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        Moving();
        TimeManipulation();
        PositionCheck();
        TimeSight();

        if (Context.Distace>Context.DistanceSight)
        {
            jumper.SwitchState(jumper.PoToPoState); 
        }

        if (Context.SecondsElapsedHearing<=0 || Context.SecondsElapsedSight<=0)
        {
            jumper.SwitchState(jumper.ChaseState);
        }

    }

    private void Moving()
    {
        if (Context.transform.position == Context.Pos1)
        {
            Context.NextPos = Context.Pos2;
            Context.CheckVector = new Vector2(1f, 0f);
            Context.Mutant.transform.localScale = new Vector3(-5, 5, 0);
        }

        if (Context.transform.position == Context.Pos2)
        {
            Context.NextPos = Context.Pos1;
            Context.CheckVector = new Vector2(-1f, 0f);
            Context.Mutant.transform.localScale = new Vector3(5, 5, 0);
        }
        Context.transform.position = Vector3.MoveTowards(Context.transform.position, Context.NextPos, Context.Speed * Time.deltaTime);
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
        Context.PlayerPosition = Context.Player2.position;
        Context.JumperPosition = Context.Jumper.position;

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
