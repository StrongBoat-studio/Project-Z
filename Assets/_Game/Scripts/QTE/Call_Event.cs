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
    private bool _isSuccess;

    


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(_square.active==false)
            {

                _square.SetActive(true);
                QTEManager.Instance.QTEStart(QTEManager.Caller.Crafting, 3);

            }
            else
            {
                _square.SetActive(false);

            }
        }

        if (_square.active == true)
        {
            _caller=QTEManager.Instance.QTEAction(QTEManager.Caller.Crafting, 1, _isSuccess);
        }


    }

    void Result()
    {
        if (_caller==QTEManager.Caller.Crafting && _isSuccess)
        {
            Debug.Log("win");
        }
    }



}
