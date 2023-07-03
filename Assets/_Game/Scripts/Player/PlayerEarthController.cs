using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEarthController : MonoBehaviour
{
    [SerializeField] private List<int> _earthLocationsId;

    private bool _isEarth=true;
    private Scene[] _scenes;

    // Update is called once per frame
    void Update()
    {
        _scenes = SceneManager.GetAllScenes();

        foreach(int i in _earthLocationsId)
        {
            Debug.Log(i);
        }

        IsEarthCheck();
    }

    private void IsEarthCheck()
    {
        for (int i = 0; i < _scenes.GetLength(0); i++)
        {
            if (_earthLocationsId.Contains(_scenes[i].buildIndex))
            {
                _isEarth = true;
                return;
            }
        }

       _isEarth = false;
    }

    public bool IsEarth()
    {
        return _isEarth;
    }
}
