using UnityEngine;

public abstract class BossBaseState
{
    BossStateManager context;

    public BossStateManager Context { get { return context; } set { context = value; } }

    public abstract void EnterState(BossStateManager boss);
    public abstract void UpdateState(BossStateManager boss);
}
