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
        Moving();
        TimeManipulation();

        if (Context.Distace > Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.PoToPoState);
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

        if (Context.Distace <=6  && !Context.Zone2)
        {
            Context.SecondsElapsedHearing -= 2;
            Context.Zone2 = true;
        }

    }
}
