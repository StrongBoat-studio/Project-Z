using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderRoomDoor : MonoBehaviour
{
    private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator = collision.GetComponentInChildren<Animator>();

        _animator.SetBool("IsTriggerDoor2", true);
    }
}
