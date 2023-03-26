using UnityEngine;

public class WalkerAttackState : WalkerBaseState
{
    private float _time = 1f;

    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Attack State");
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        AttackP();
        AttackM();

        if (Context.Distance > Context.DistanceChase)
        {
            walker.SwitchState(walker.PoToPoState);
            Context.Animator.SetBool("IsAttack1", false);
        }

    }

    private void AttackP()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Context.MLife -= 10;
        }
    }

    private void AttackM()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Context.Animator.SetBool("IsAttack1", true);
            Context.PLife -= 10;
            _time = 1f;
        }

    }
}
