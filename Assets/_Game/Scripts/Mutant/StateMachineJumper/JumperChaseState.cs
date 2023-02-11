using UnityEngine;

public class JumperChaseState : JumperBaseState
{
    private Vector3 _target;
    public override void EnterState(JumperStateManager jumper)
    {
        Debug.Log("Chase State");

        
    }

    public override void UpdateState(JumperStateManager jumper)
    {
        Chasing();

        if (Context.Distace>Context.DistanceHearing)
                {
                    jumper.SwitchState(jumper.PoToPoState);
                }

        if(Context.Distace<=Context.DistanceAttack)
        {
            jumper.SwitchState(jumper.AttackState);
        }
    }

    private void Chasing()
    {
        _target = new Vector3(Context.Target.position.x, Context.transform.position.y, Context.transform.position.z);

        Context.transform.position = Vector2.MoveTowards(Context.transform.position, _target, (Context.Speed + 1f) * Time.deltaTime);
    }


}
