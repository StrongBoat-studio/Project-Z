using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Heavy : MonoBehaviour
{
    [SerializeField] private Transform _pos1, _pos2;
    [SerializeField] private Transform _startPos;
    [SerializeField] private GameObject _mutant, _player;

    [Header("Stealth Settings")]
    [Range(0f, 10f)] [SerializeField] private float _speed;
    [Range(0f, 10f)] [SerializeField] private float _distanceChase, _distanceAttack;
    [Range(0f, 10f)] [SerializeField] private float _secondOfSleep;

    [Header("Stealth Chack")]
    [SerializeField] private float _secondsElapsed;
    [SerializeField] private float _distance;

    private Transform _target;

    private bool _chase=false;
    private bool _zone1=false, _zone2=false, _zone3=false;

    Vector3 nextPos;

    private void Start()
    {
        nextPos = _startPos.position;
        _secondsElapsed = _secondOfSleep;
    }

    private void Update()
    {
        _distance = Vector3.Distance(_mutant.transform.position, _player.transform.position);

        if (_distance > _distanceChase)   
        {
            _chase = false;
            _zone1 = false;
            _zone2 = false;
            _zone3 = false;
            _secondsElapsed = _secondOfSleep;
        }

        if (_distance <= _distanceChase)
        {
            _chase = true;
        }

        if (_secondsElapsed > 0) //Moving from one position to another
        {
            Moving();
        }

        if (_distance <= _distanceChase && _secondsElapsed <= 0) //Chasing the Player after a certain amount of time
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

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_pos1.position, _pos2.position);
    }

    private void Moving()
    {
        if (transform.position == _pos1.position)
        {
            nextPos = _pos2.position;
        }

        if (transform.position == _pos2.position)
        {
            nextPos = _pos1.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
    }

    private void Chasing()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.position = Vector2.MoveTowards(transform.position, _target.position, (_speed + 0.5f) * Time.deltaTime);
    }

    private void TimeManipulation()
    {
        _secondsElapsed -= Time.deltaTime;

        if (_distance<=5 && !_zone1)
        {
            Debug.Log("-2");
            _secondsElapsed -= 2;
            _zone1 = true;
        }

        if (_distance <= 4 && !_zone2)
        {
            _secondsElapsed -= 1;
            _zone2 = true;
        }

        if (_distance <= 3 && !_zone3)
        {
            _secondsElapsed = 0;
            _zone3 = true;
        }
    }
}

