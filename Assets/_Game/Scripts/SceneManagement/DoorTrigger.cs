using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform _bottomColliderTransform;
    private Animator _animator;
    private Transform _transform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _bottomColliderTransform.position = new Vector2(_bottomColliderTransform.position.x, -2.6f);

        _animator = collision.GetComponentInChildren<Animator>();
        _transform = collision.GetComponent<Transform>();

        //_transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        _animator.SetBool("IsTriggerDoor1", true);
    }
}
