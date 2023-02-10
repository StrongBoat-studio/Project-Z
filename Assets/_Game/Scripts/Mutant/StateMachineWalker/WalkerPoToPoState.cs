using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WalkerPoToPoState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Point To Point State");

        Context.Zone1 = false;
        Context.Zone2 = false;
        Context.Zone3 = false;
        
        Context.Alert.SetActive(false);
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        Moving();
        PositionCheck();
        SecondsElapsedAndDistanceChase();
        CrounchingCheck();

        if(Context.Distance <= Context.DistanceLineOfHearing)
        {
            walker.SwitchState(walker.HearingState);
        }

    }

    private void Moving()
    {
        if (Context.transform.position == Context.Pos1)
        {
            Context.NextPos = Context.Pos2;
            Context.CheckVector = new Vector2(1f, 0f);
            Context.Mutant.transform.localScale = new Vector3(-3, 3, 0);
        }

        if (Context.transform.position == Context.Pos2)
        {
            Context.NextPos = Context.Pos1;
            Context.CheckVector = new Vector2(-1f, 0f);
            Context.Mutant.transform.localScale = new Vector3(3, 3, 0);
        }

        Context.transform.position = Vector3.MoveTowards(Context.transform.position, Context.NextPos, Context.Speed * Time.deltaTime);
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck() 
    {
        Context.PlayerPosition = Context.Player2.position;
        Context.WalkerPosition = Context.Walker.position;

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
        if (Context.Movement.GetMovementStates().Contains(Movement.MovementState.Crouching))
        {
            Context.SecondsHearingElapsed = Context.SecondHearingCrounching;

        }
        else Context.SecondsHearingElapsed = Context.SecondHearingNormally;
    }
}
