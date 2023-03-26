using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class QTE_Manager : MonoBehaviour
{
    [Header("Letter objects")]
    [SerializeField] private GameObject j;
    [SerializeField] private GameObject k;
    [SerializeField] private GameObject l;
    [SerializeField] private GameObject m;
    [SerializeField] private GameObject o;
    [SerializeField] private GameObject p;
    [SerializeField] private GameObject t;
    [SerializeField] private GameObject u;


    [Header("QTE Settings")]
    [SerializeField][Range(0f, 1f)] private float _frameChange = 0.1f;
    [SerializeField][Range(0f, 1f)] private float _timeThreshold = 0.05f;
    [SerializeField][Range(1f, 10f)] private int _howManyLetters = 5;

    //QTE Variables
    private float _fillAmount = 0;
    private float _timeThresholdV = 0;
    private bool _eventSuccess = true;
    private GameObject[] _array;
    private int _lottery = 0;
    public System.Random generator = new System.Random();
    private int _howManyLettersV = 0;

    //Action
    private Action _action = null;


    void Start()
    {
        _array = new GameObject[] { j, k, l, m, o, p, t, u };
    }

    public void QteManager()
    {
        if (_howManyLettersV < _howManyLetters)
        {
            PrepareAction(EventSuccess, QTE);
            _action?.Invoke();
        }
    }

    public void ResetVariables()
    {
        _fillAmount = 0;
        _timeThresholdV = 0;
        _howManyLettersV = 0;
    }

    private void EventSuccess()
    {
        _eventSuccess = false;
        _lottery = generator.Next(8);
        _array[_lottery].SetActive(true);
    }

    private void QTE()
    {
        SwitchLetter();

        _timeThresholdV += Time.deltaTime;

        if (_timeThresholdV > _timeThreshold)
        {
            _timeThresholdV = 0;
            _fillAmount -= .02f;
        }

        if (_fillAmount < 0)
        {
            _fillAmount = 0;
        }

        if (_fillAmount >= 1 && _eventSuccess == false)
        {
            _eventSuccess = true;
            _array[_lottery].SetActive(false);
            _fillAmount = 0;
            _howManyLettersV++;
        }

        _array[_lottery].transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = _fillAmount;
    }

    private void PrepareAction(Action eventSuccess, Action qte)
    {
        if (_eventSuccess)
        {
            _action = eventSuccess;

        }
        else
        {
            _action = qte;
        }
    }

    private void SwitchLetter()
    {
        switch (_lottery)
        {
            case 0:
                {
                    if (Input.GetKeyDown("j"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 1:
                {
                    if (Input.GetKeyDown("k"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 2:
                {
                    if (Input.GetKeyDown("l"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 3:
                {
                    if (Input.GetKeyDown("m"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 4:
                {
                    if (Input.GetKeyDown("o"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 5:
                {
                    if (Input.GetKeyDown("p"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 6:
                {
                    if (Input.GetKeyDown("t"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            case 7:
                {
                    if (Input.GetKeyDown("u"))
                    {
                        _fillAmount += _frameChange;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


}
