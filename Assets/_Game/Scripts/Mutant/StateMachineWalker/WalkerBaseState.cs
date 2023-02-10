using UnityEngine;

public abstract class WalkerBaseState
{
    WalkerStateManager context;

    public WalkerStateManager Context { get { return context; } set { context = value; } }

    public abstract void EnterState(WalkerStateManager walker);
    public abstract void UpdateState(WalkerStateManager walker);
}
