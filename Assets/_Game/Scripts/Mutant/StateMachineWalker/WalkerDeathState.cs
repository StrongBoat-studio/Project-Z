using UnityEngine;

public class WalkerDeathState : WalkerBaseState
{
    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("I`m dead");
    }

    public override void UpdateState(WalkerStateManager walker)
    {

    }
}
