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
        SetPosition();
        Moving();
        PositionCheck();
        SecondsElapsedAndDistanceChase();
        HearingTime();

        if (Context.Distance > Context.DistanceLineOfHearing)
        {
            walker.SwitchState(walker.PoToPoState);
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

    private void SetPosition()
    {
        if (Context.transform.position.x >= Context.Pos1.x)
        {
            Context.NextPos = Context.Pos2;
            Context.CheckVector = new Vector2(1f, 0f);
            Context.Mutant.transform.localScale = new Vector3(1, 1, 0);
        }

        if (Context.transform.position.x <= Context.Pos2.x)
        {
            Context.NextPos = Context.Pos1;
            Context.CheckVector = new Vector2(-1f, 0f);
            Context.Mutant.transform.localScale = new Vector3(-1, 1, 0);
        }
    }

    private void Moving()
    {
        if (Context.NextPos == Context.Pos1)
        {
            Context.Rigidbody.velocity = new Vector2(Context.Speed, Context.transform.position.y);
        }
        else
        {
            Context.Rigidbody.velocity = new Vector2(-Context.Speed, Context.transform.position.y);
        }

    }

    private void HearingTime()
    {
        Context.SecondsHearingElapsed -= Time.deltaTime;
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
}
