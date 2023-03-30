using UnityEngine;
using System;

public class WalkerAttackState : WalkerBaseState
{
    private float _time = 1f;
    public System.Random generator = new System.Random();
    private double _anim;

    public override void EnterState(WalkerStateManager walker)
    {
        Debug.Log("Attack State");
    }

    public override void UpdateState(WalkerStateManager walker)
    {
        AttackM();
        PositionCheck();
        //LookAtThePlayer();

        if (Context.Distance > Context.DistanceChase)
        {
            walker.SwitchState(walker.PoToPoState);
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
            
            Context.PLife -= 10;
            _time = 1f;
        }

    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        Context.PlayerPosition = Context.Player2.position;
        Context.WalkerPosition = Context.Walker.position;

        Context.Direction = Context.PlayerPosition - Context.WalkerPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }

    private void LookAtThePlayer()
    {
        if(Context.Player2.transform.localScale.x==1)
        {
            Context.Walker.transform.localScale = new Vector3(-1f, 1f, 0f);
        }
        if (Context.Player2.transform.localScale.x == -1)
        {
            Context.Walker.transform.localScale = new Vector3(1f, 1f, 0f);
        }
    }

}
