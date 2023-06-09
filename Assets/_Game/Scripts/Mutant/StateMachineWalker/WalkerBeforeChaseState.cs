using Unity.VisualScripting;
using UnityEngine;

public class WalkerBeforeChaseState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Before Chase State");

        Context.Alert.SetActive(true);
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        SetPosition();
        Moving();
        PositionCheck();
        TimeManipulation();

        if (Context.Distance>Context.DistanceChase)
        {
            walker.SwitchState(walker.PoToPoState);
        }

        if(Context.SecondsElapsed<=0)
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

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        Context.PlayerPosition = Context.Player2.position;
        Context.WalkerPosition = Context.Walker.position;

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
