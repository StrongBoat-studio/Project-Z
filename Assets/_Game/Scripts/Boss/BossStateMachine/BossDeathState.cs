using UnityEngine;

public class BossDeathState : BossBaseState
{
    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss death state");
        Context.Animator.SetBool("IsWalking", false);
        Context.MonoBehaviour.StopAllCoroutines();
        Context.AddItem();
        Context.Destroy();
    }

    public override void UpdateState(BossStateManager boss)
    {

    }
}
