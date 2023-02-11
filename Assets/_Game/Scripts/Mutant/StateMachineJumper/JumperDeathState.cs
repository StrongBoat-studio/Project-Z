using UnityEngine;

public class JumperDeathState : JumperBaseState
{
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("I`m dead");
    }

    public override void UpdateState(JumperStateManager jumper)
    {

    }
}
