using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    private bool _isQTEStarted = false;

    private void Awake()
    {
        if (QTEManager.Instance != null)
        {
            QTEManager.Instance.OnWinQTE += OnWinQTE;
            QTEManager.Instance.OnFailQTE += OnFailQTE;
        }
    }

    private void Update()
    {
        if(_isQTEStarted)
        {
            QTEManager.Instance.QTEAction(QTEManager.Caller.FaightController, 2);
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
        if (caller == QTEManager.Caller.FaightController)
        {
            GetComponent<Animator>().SetBool("JurijWin", true);
        }
    }

    private void OnFailQTE(QTEManager.Caller caller)
    {
        if (caller == QTEManager.Caller.FaightController)
        {
            GetComponent<Animator>().SetBool("BorisWin", true);
        }
    }

    private void OnDestroy()
    {
        QTEManager.Instance.OnWinQTE -= OnWinQTE;
        QTEManager.Instance.OnFailQTE -= OnFailQTE;
    }
}
