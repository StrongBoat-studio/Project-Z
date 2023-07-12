using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBack : MonoBehaviour
{
    [SerializeField] private RoomLoader _roomLoader;
    [SerializeField] private Transform _bottomCollider;
    [SerializeField] private float _colliderPositionY=-3.0f;

    private void Update()
    {
        _bottomCollider.position = new Vector2(_bottomCollider.position.x, _colliderPositionY);
    }

    public void ChangeLocation()
    {
        _roomLoader.CursorClick();
    }
}
