using UnityEngine;

public class HeavyDeathState : HeavyBaseState
{
    public override void EnterState(HeavyStateManager heavy)
    {
        Debug.Log("Death State");
    }

    public override void UpdateState(HeavyStateManager heavy)
    {

    }
}
