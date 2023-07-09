using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceToTheShip : MonoBehaviour
{
    [SerializeField] private RoomLoader _roomLoader;
    
    public void ChangeLocation()
    {
        _roomLoader.CursorClick();
    }
}
