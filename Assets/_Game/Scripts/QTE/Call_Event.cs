using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Call_Event : MonoBehaviour
{
    [SerializeField] GameObject _square;

    //Check Variables
    private QTEManager.Caller _caller;
    private bool _oneCall = false;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(_square.activeSelf == false)
            {

                _square.SetActive(true);
                QTEManager.Instance.QTEStart(QTEManager.Caller.Crafting, 5);

            }
            else
            {
                _square.SetActive(false);
                QTEManager.Instance.QTEStop();

            }
        }

        if (_square.activeSelf == true)
        {
            _caller=QTEManager.Instance.QTEAction(QTEManager.Caller.Crafting, 1);
        }

        Result();

    }

    private void Result()
    {
        if (_caller == QTEManager.Caller.Crafting && QTEManager.Instance._isSuccess == 1)
        {
            QTEManager.Instance._isSuccess = 0;
            Debug.Log("win");
        }

        if (_caller == QTEManager.Caller.Crafting && QTEManager.Instance._isSuccess == -1)
        {
            QTEManager.Instance._isSuccess = 0;
            Debug.Log("fail");
        }
    }



}
