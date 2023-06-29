using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMutantInit
{
    int GetLife();
    void SetLife(int life);
    Vector3 GetPosition();
    void SetPosition(Vector3 position);
    void SetDefaultState();
    LevelManagerData.MutantType GetMutantType();
    void UpdateInit();
}
