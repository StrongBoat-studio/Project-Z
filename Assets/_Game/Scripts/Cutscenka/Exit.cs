using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private RoomLoader _roomLoader;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _roomLoader.CursorClick();
        }
    }

    public void ExitCutscene()
    {
        _roomLoader.CursorClick();
    }
}
