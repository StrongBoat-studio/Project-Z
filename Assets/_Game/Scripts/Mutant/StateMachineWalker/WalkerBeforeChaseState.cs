using Unity.VisualScripting;
using UnityEngine;

public class WalkerBeforeChaseState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Context.Alert.SetActive(true);
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        PositionCheck();
        TimeManipulation();

        if (Context.Distance>Context.DistanceChase)
        {
            walker.SwitchState(walker.PatrollingState);
        }

        if(Context.SecondsElapsed<=0)
        {
            walker.SwitchState(walker.ChaseState);
        }
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

    private void TimeManipulation()
    {
            Context.SecondsElapsed -= Time.deltaTime;

            if (Context.Distance <= 4 && Context.DotPro > 0 && !Context.Zone1)
            {
                Context.SecondsElapsed -= 2;
                Context.Zone1 = true;
            }

            if (Context.Distance <= 3 && Context.DotPro > 0 && !Context.Zone2)
            {
                Context.SecondsElapsed -= 2;
                Context.Zone2 = true;
            }

            if (Context.Distance <= 2 && Context.DotPro > 0 && !Context.Zone3)
            {
                Context.SecondsElapsed = 0;
                Context.Zone3 = true;
            }
    }
}
