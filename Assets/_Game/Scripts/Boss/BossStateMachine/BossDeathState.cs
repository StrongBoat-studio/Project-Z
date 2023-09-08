using UnityEngine;

public class BossDeathState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("death attack state");
    }

    public override void UpdateState(BossStateManager boss)
    {

    }
}
