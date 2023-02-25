using UnityEngine;

public abstract class HeavyBaseState
{
    HeavyStateManager context;

    public HeavyStateManager Context { get { return context; } set { context = value; } }

    public abstract void EnterState(HeavyStateManager heavy);
    public abstract void UpdateState(HeavyStateManager heavy);

}
