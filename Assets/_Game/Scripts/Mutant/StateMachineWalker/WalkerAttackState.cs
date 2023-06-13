using UnityEngine;
using System;

public class WalkerAttackState : WalkerBaseState
{
    private float _time = 1f;
    public System.Random generator = new System.Random();
    private double _anim;
    private Vector3 _target;

    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Attack State");
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        AttackM();
        PositionCheck();

        if (Context.Distance > Context.DistanceChase)
        {
            walker.SwitchState(walker.PatrollingState);
            Context.Animator.SetBool("IsAttack1", false);
            Context.Animator.SetBool("InAttack2", false);
        }
    }

    private void AttackM()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _anim = generator.NextDouble();
            if(_anim<.5f)
            {
                Context.Animator.SetBool("IsAttack1", true);
                Context.Animator.SetBool("InAttack2", false);
            }
            else
            {
                Context.Animator.SetBool("IsAttack1", false);
                Context.Animator.SetBool("InAttack2", true);
            }
            _time = 1f;
        }
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        Context.PlayerPosition = Context.Player.transform.position;
        Context.WalkerPosition = Context.Mutant.transform.position;

        Context.Direction = Context.PlayerPosition - Context.WalkerPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
