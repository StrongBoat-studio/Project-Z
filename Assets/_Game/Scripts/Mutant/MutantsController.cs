using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantsController : MonoBehaviour
{
    [SerializeField] private GameObject[] _mutantPrefebs;
    [SerializeField] private int _numberOfMutants = 0;
    [SerializeField] private float _yPositionWalker;
    [SerializeField] private float _yPositionJumper;
    [SerializeField] private int _xMin;
    [SerializeField] private int _xMax;

    private GameObject _currentPrefab;

    void Update()
    {
        if(this.gameObject.transform.childCount<_numberOfMutants)
        {
            int randomValue = Random.Range(1, 3);
            int x = Random.Range(_xMin, _xMax);
            int patrolingXLeft = Random.Range(_xMin, x);
            int patrolingXRight = Random.Range(x, _xMax);

            if (randomValue==1)
            {
                _currentPrefab = Instantiate(_mutantPrefebs[0], new Vector2(patrolingXLeft, _yPositionWalker), Quaternion.identity, this.gameObject.transform);
                _currentPrefab.GetComponent<WalkerPatrolling>().SetRange(patrolingXLeft, patrolingXRight);
                AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantIdle, new Vector2(patrolingXLeft, _yPositionWalker));
                return;
            }

            if(randomValue==2)
            {
                _currentPrefab = Instantiate(_mutantPrefebs[1], new Vector2(patrolingXRight, _yPositionJumper), Quaternion.identity, this.gameObject.transform);
                _currentPrefab.GetComponent<JumperPatrolling>().SetRange(patrolingXLeft, patrolingXRight);
                AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantIdle, new Vector2(patrolingXLeft, _yPositionWalker));
                return;
            }
        }
    }
}
