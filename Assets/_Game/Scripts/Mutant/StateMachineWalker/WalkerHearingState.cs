using UnityEngine;

public class WalkerHearingState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Hearing State");
        Context.Alert.SetActive(true);
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        PositionCheck();
        SecondsElapsedAndDistanceChase();
        HearingTime();

        if (Context.Distance > Context.DistanceLineOfHearing)
        {
            walker.SwitchState(walker.PatrollingState);
        }

        if (Context.Distance < Context.DistanceChase)
        {
            walker.SwitchState(walker.BeforeChaseState);
        }

        if (Context.SecondsHearingElapsed < 0)
        {
            walker.SwitchState(walker.ChaseState);
        }
    }

    private void HearingTime()
    {
        Context.SecondsHearingElapsed -= Time.deltaTime;
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        if (Context.Player == null)
        {
            return;
        }

        Context.PlayerPosition = Context.Player.transform.position;
        Context.WalkerPosition = Context.Mutant.transform.position;

        Context.Direction = Context.PlayerPosition - Context.WalkerPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }

    private void SecondsElapsedAndDistanceChase()
    {
        if (Context.DotPro >= 0)
        {
            Context.SecondsElapsed = Context.SecondOfSleepFront;
            Context.DistanceChase = Context.DistanceChaseFront;
        }
        else
        {
            Context.SecondsElapsed = Context.SecondOfSleepBack;
            Context.DistanceChase = Context.DistanceChaseBack;
        }
    }
}
