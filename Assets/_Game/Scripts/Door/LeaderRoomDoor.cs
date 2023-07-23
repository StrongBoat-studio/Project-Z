using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderRoomDoor : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private BoxCollider2D _boxCollider2;
    [SerializeField] private RoomLoader _roomLoader;

    private void Awake()
    {
        _boxCollider2.enabled = false;
        _roomLoader.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.movement.IsGrounded()) return;

        _boxCollider2.enabled = true;
        _roomLoader.enabled = true;
        _animator = collision.GetComponentInChildren<Animator>();

        _animator.SetBool("IsTriggerDoor2", true);
    }
}
