using UnityEngine;

public class BossQTEState : BossBaseState
{
    private bool _isQTEStarted = false;
    private bool _canTransition = false;

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("boss qte state");

        _canTransition = false;
        StartQTE();

        if (QTEManager.Instance != null)
        {
            QTEManager.Instance.OnWinQTE += OnWinQTE;
            QTEManager.Instance.OnFailQTE += OnFailQTE;
        }

        Context.Animator.SetBool("IsWalking", false);
    }

    public override void UpdateState(BossStateManager boss)
    {
        if (_isQTEStarted)
        {
            QTEManager.Instance.QTEAction(QTEManager.Caller.FaightController, Random.Range(1,4));
        }

        if(_canTransition==true)
        {
            Context.Animator.SetBool("IsWalking", true);
            _isQTEStarted = false;
            Context.nextAttackId = 1;
            QTEManager.Instance.OnWinQTE -= OnWinQTE;
            QTEManager.Instance.OnFailQTE -= OnFailQTE;
            boss.SwitchState(boss.AttackState);
            _canTransition = false;
        }
    }

    public void StartQTE()
    {
        if (_isQTEStarted) return;

        QTEManager.Instance.QTEStart(QTEManager.Caller.FaightController, 3);
        _isQTEStarted = true;
    }

    private void OnWinQTE(QTEManager.Caller caller)
    {
        Context.TakeDamage(Random.Range(5, 11), 0);
        _canTransition = true;
    }

    private void OnFailQTE(QTEManager.Caller caller)
    {
        GameManager.Instance.Player.TakeDamage(Random.Range(5, 11));
        _canTransition = true;
    }
}
