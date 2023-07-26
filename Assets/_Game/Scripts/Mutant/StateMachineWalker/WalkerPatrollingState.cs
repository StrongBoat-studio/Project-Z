using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WalkerPatrollingState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Context.Zone1 = false;
        Context.Zone2 = false;
        Context.Zone3 = false;
        
        Context.Alert.SetActive(false);
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        PositionCheck();
        SecondsElapsedAndDistanceChase();
        CrounchingCheck();

        if(Context.Distance <= Context.DistanceLineOfHearing)
        {
           walker.SwitchState(walker.HearingState);
        }

    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck() 
    {
        if (Context.Player == null || Context.Mutant==null)
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

    private void CrounchingCheck()
    {
        if(Context.Movement==null)
        {
            return;
        }

        if (Context.Movement.GetMovementStates().Contains(Movement.MovementState.Crouching))
        {
            Context.SecondsHearingElapsed = Context.SecondHearingCrounching;

        }
        else Context.SecondsHearingElapsed = Context.SecondHearingNormally;
    }
}
