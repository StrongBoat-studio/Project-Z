using UnityEngine;

public class JumperPoToPoState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Point To Point State");

        Context.Zone1 = false;
        Context.Zone2 = false;

        Context.SecondsElapsedHearing = Context.SecondsHearing;

        Context.M.SetActive(false);
        Context.Alert.SetActive(false);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        Moving();

        if(Context.Distace<=Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.HearingState);
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
}
