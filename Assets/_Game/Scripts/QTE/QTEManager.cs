using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance { get; private set; }

    [Header("Letter objects")]
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject j;
    [SerializeField] private GameObject k;
    [SerializeField] private GameObject l;
    [SerializeField] private GameObject m;
    [SerializeField] private GameObject o;
    [SerializeField] private GameObject p;
    [SerializeField] private GameObject t;
    [SerializeField] private GameObject u;


    [Header("QTE Settings")]
    [SerializeField] [Range(0f, 1f)] private float _frameChange = 0.1f;
    [SerializeField] [Range(0f, 1f)] private float _timeThreshold = 0.05f;

    //Enum
    public enum Caller
    {
        Crafting=1,
    }


    //QTE Variables
    private float _fillAmount = 0;
    private float _timeThresholdV = 0;
    private bool _eventSuccess = true;
    private GameObject[] _array;
    private int _lottery = 0;
    public System.Random generator = new System.Random();
    [SerializeField] private int _howManyLettersV = 0;
    [SerializeField] private float _timeForQTECompleted;
    public int _isSuccess=0;

    //Action
    private Action _action = null;

    //Events
    public event Action<int> _qte;
    public event Action<Caller> _qteResult = null;
    public event Action<float> _countingDown = null;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        _array = new GameObject[] { j, k, l, m, o, p, t, u };

    }



    //Update QTE state by calling QteManager(int howManyLetters)
    public Caller QTEAction(Caller caller, int howManyLetters)
    {
        _qte?.Invoke(howManyLetters);

        _countingDown?.Invoke(_timeForQTECompleted);

        _qteResult?.Invoke(caller);

        return caller;
    }


    //Checks whether QTE is finished or not and calls the appropriate action
    public void QteManager(int howManyLetters)
    {
        if (_howManyLettersV < howManyLetters)
        {
            PrepareAction(EventSuccess, QTE);
            _action?.Invoke();
        }
        if(_howManyLettersV == howManyLetters)
        {
            _qteResult += QTESuccess;
        }

        
    }

    //Subscribes to the event and resets the variables
    public void QTEStart( Caller caller, float timeForQTECompleted)
    {
        _qte += QteManager;
        _fillAmount = 0;
        _timeThresholdV = 0;
        _howManyLettersV = 0;
        _eventSuccess = true;

        _timeForQTECompleted = timeForQTECompleted;
        _countingDown += CountingDown;
    }

    //Stops QTE
    public void QTEStop()
    {
        _qte -= QteManager;
        _qteResult -= null;
        _countingDown -= CountingDown;
        _isSuccess = 0;

        _array[_lottery].SetActive(false);
        button.SetActive(false);
    }

    private void EventSuccess()
    {
        _eventSuccess = false;
        _lottery = generator.Next(8);
        _array[_lottery].SetActive(true);
        button.SetActive(true);
    }


    //The essence of QTE action
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
            button.SetActive(false);
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

    private void CountingDown(float timeForQTECompleted)
    {
        if (_timeForQTECompleted > 0)
        {
            _timeForQTECompleted -= Time.deltaTime;
        }
        else
        {
            _qteResult += QTEFail;
        }
    }

    private void QTEFail(Caller caller)
    {
        _qte -= QteManager;
        _qteResult -= QTEFail;
        _countingDown -= CountingDown;
        _isSuccess = -1;

        _array[_lottery].SetActive(false);
        button.SetActive(false);
        //Debug.Log("fail");
    }

    private void QTESuccess(Caller caller)
    {
        _qte -= QteManager;
        _qteResult -= QTESuccess;
        _countingDown -= CountingDown;
        _isSuccess = 1;

        _array[_lottery].SetActive(false);
        button.SetActive(false);
        //Debug.Log("win");
    }





}
