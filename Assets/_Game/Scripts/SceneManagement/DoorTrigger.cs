using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform _bottomColliderTransform;
    private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.movement.IsGrounded() || GameManager.Instance.player.position.x < 6) return;

        _animator = collision.GetComponentInChildren<Animator>();

        _animator.SetBool("IsTriggerDoor1", true);
    }
}
