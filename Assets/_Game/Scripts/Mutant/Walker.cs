using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Walker : MonoBehaviour
{
    [SerializeField] private Vector3 _pos1, _pos2, _startPos;
    [SerializeField] private GameObject _mutant, _player;
    [SerializeField] private Transform _walker, _player2;
    [SerializeField] private Movement _movement;
    
    [Header("Stealth Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed;
    [Range(0f, 10f)] [SerializeField] private float _distanceChaseFront, _distanceChaseBack,_distanceAttack;
    [Range(0f, 10f)] [SerializeField] private float _secondOfSleepFront, _secondOfSleepBack;
    [Range(0f, 10f)] [SerializeField] private float _distanceLineOfSight, _secondSightNormally, _secondSightCrounching;

    [Header("Stealth Chack")]
    [SerializeField] private float _secondsElapsed;  
    [SerializeField] private float _secondsSightElapsed;
    [SerializeField] private float _distance;
    [SerializeField] private float dotPro;
    [SerializeField] private bool _chaseAfterSight = false; 

    private Vector2 _playerPosition;
    private Vector2 _walkerPosition;
    private Vector2 _direction;
    private Vector2 _checkVector = new Vector2(1f, 0f);

    private Transform _target;
    private float _distanceChase;
    

    private bool _chase=false;
    private bool _zone1=false, _zone2=false, _zone3=false;
    

    Vector3 nextPos;

    private void Start()
    {
        nextPos = _startPos;
        _secondsElapsed = _secondOfSleepFront;
        _secondsSightElapsed = _secondSightNormally; 
    }

    private void Update()
    {
        PositionCheck();
        LineOfSight();

        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);

        if (_distance > _distanceChase)   
        {
            _chase = false;
            _zone1 = false;
            _zone2 = false;
            _zone3 = false;
            SecondsElapsedAndDistanceChase();
        }

        if (_distance <= _distanceChase)
        {
            _chase = true;
        }

        if (_secondsElapsed > 0) //Moving from one position to another
        {
            Moving();
        }

        if ((_distance <= _distanceChase && _secondsElapsed <= 0) || (_distance<=_distanceLineOfSight && _secondsSightElapsed<=0)) //Chasing the Player after a certain amount of time
        {
            Chasing();
        }

        if (_distance <= _distanceAttack && _secondsElapsed <= 0) //Attacking the Player
        {
            Debug.Log("Atak");

        }

        if (_chase)
        {
            TimeManipulation();
        }

        if (_chaseAfterSight)
        {
            SightTime();
        }


        Debug.DrawLine(Vector2.zero, _checkVector, Color.red);
        Debug.DrawLine(Vector2.zero, _direction, Color.blue);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_pos1, _pos2);
    }

    private void Moving()
    {
        if (transform.position == _pos1)
        {
            nextPos = _pos2;
            _checkVector = new Vector2(1f, 0f);
            _mutant.transform.localScale = new Vector3(-3, 3, 0);
        }

        if (transform.position == _pos2)
        {
            nextPos = _pos1;
            _checkVector = new Vector2(-1f, 0f);
            _mutant.transform.localScale = new Vector3(3, 3, 0);
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
    }

    private void Chasing()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.position = Vector2.MoveTowards(transform.position, _target.position, (_speed + 0.5f) * Time.deltaTime);
    }

    private void PositionCheck() //Checking whether the Player is in front of or behind the Mutant
    {
        _playerPosition = _player2.position;
        _walkerPosition = _walker.position;

        _direction = _playerPosition - _walkerPosition;
        _direction.Normalize();

        dotPro = Vector2.Dot(_direction, _checkVector);
    }

    private void TimeManipulation()
    {
        if(_secondsElapsed>0)
        {
            _secondsElapsed -= Time.deltaTime;

            if (_distance<=5 && !_zone1 && dotPro>0)
            {
                _secondsElapsed -= 2;
                _zone1 = true;
            }

            if (_distance <= 4 && !_zone2 && dotPro > 0)
            {
                _secondsElapsed -= 2;
                _zone2 = true;
            }

            if (_distance <= 3 && !_zone3 && dotPro > 0 )
            {
                _secondsElapsed = 0;
                _zone3 = true;
            }
        }

    }

    private void SecondsElapsedAndDistanceChase()
    {
        if (dotPro >= 0)
        {
            _secondsElapsed = _secondOfSleepFront;
            _distanceChase = _distanceChaseFront;
        }
        else
        {
            _secondsElapsed = _secondOfSleepBack;
            _distanceChase = _distanceChaseBack;
        }
    }

    private void CrounchingCheck()
    {
        if (_movement.GetMovementStates().Contains(Movement.MovementState.Crouching))
        {
            _secondsSightElapsed = _secondSightCrounching;

        }
        else _secondsSightElapsed = _secondSightNormally;
    }

    private void LineOfSight()
    {
        if (_distance <= _distanceLineOfSight)
        {
            _chaseAfterSight = true;
        }
        else
        {
            _chaseAfterSight = false;
            CrounchingCheck();
        }
    }

    private void SightTime()
    {
        if(_secondsSightElapsed>0)
        {
            _secondsSightElapsed -= Time.deltaTime;
        }
    }
}

