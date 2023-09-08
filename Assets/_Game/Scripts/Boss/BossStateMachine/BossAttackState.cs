using UnityEngine;

public class BossAttackState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss attack state");
    }

    public override void UpdateState(BossStateManager boss)
    {

    }
}
