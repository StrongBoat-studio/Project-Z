using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitingEmptyLocations : MonoBehaviour
{
    [SerializeField] private RoomLoader _roomLoader;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _roomLoader.CursorClick();
        }
    }
}
