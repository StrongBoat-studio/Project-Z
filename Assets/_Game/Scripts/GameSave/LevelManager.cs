using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelManagerData _levelData;

    public LevelManagerData GetLevelData()
    {
        return _levelData;
    }

    public void SetLevelData(LevelManagerData lmd)
    {
        _levelData = lmd;
    }

    public void ExecuteDataLoad()
    {
        foreach(var i in _levelData.items)
        {
            if(i.load == false)
            {
                Destroy(i.item);
            }
        }
    }
}
