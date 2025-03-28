using UnityEngine;

public class JumperAttackState : JumperBaseState 
{
    private float _time = 1f;
    public System.Random generator = new System.Random();
    private double _anim;
    private Vector3 _target;

    public override void EnterState(JumperStateManager jumper)
    {
        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantGrowl, Context.Mutant.position);
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        AttackM();
        PositionCheck();

        if (Context.Distace > Context.DistanceHearing)
        {
            jumper.SwitchState(jumper.PatrollingState);
            Context.Animator.SetBool("IsStanding", true);
        }

        if (Context.Distace > Context.DistanceAttack)
        {
            jumper.SwitchState(jumper.ChaseState);
            Context.Animator.SetBool("IsStanding", false);
        }

        if (Context.canAttack==false)
        {
            jumper.SwitchState(jumper.PatrollingState);
        }
    }

    private void AttackM()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _anim = generator.NextDouble();
            if (_anim < .5f)
            {
                Context.Animator.SetBool("IsStanding", false);
                Context.Animator.SetBool("IsAttack1", true);

                Context.Animator.SetBool("IsAttack2", false);
            }

            if (_anim > .5f)
            {
                Context.Animator.SetBool("IsAttack1", false);
                Context.Animator.SetBool("IsStanding", false);
                Context.Animator.SetBool("IsAttack2", true);

                Context.Jump();

            }

            _time = 1f;
        }
    }

    //Checking whether the Player is in front of or behind the Mutant
    private void PositionCheck()
    {
        if (Context.Player == null)
        {
            return;
        }

        Context.PlayerPosition = Context.Player.position;
        Context.JumperPosition = Context.Mutant.position;

        Context.Direction = Context.PlayerPosition - Context.JumperPosition;
        Context.Direction.Normalize();

        Context.DotPro = Vector2.Dot(Context.Direction, Context.CheckVector);
    }
}
