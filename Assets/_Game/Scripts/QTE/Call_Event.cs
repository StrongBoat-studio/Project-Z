using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Call_Event : MonoBehaviour
{
    [SerializeField] GameObject _square;
    [SerializeField] private UnityEvent _qte;
    [SerializeField] private UnityEvent _resetVariables;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(_square.activeSelf == false)
            {
                _resetVariables?.Invoke();
                _square.SetActive(true);
            }
            else
            {
                _square.SetActive(false);
            }
        }

        if(_square.activeSelf == true)
        {
            _qte?.Invoke();
        }
    }


}
