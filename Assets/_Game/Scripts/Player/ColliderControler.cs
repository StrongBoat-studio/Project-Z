using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderControler : MonoBehaviour
{
    [SerializeField] BoxCollider2D _playerBoxCollider2D;
    [SerializeField] BoxCollider2D _animBoxCollider2D;

    [SerializeField] Vector2 player_size;
    [SerializeField] Vector2 anim_size;

    // Update is called once per frame
    void Update()
    {
        player_size = _playerBoxCollider2D.size;
        anim_size = _playerBoxCollider2D.size;

        _playerBoxCollider2D.size = _animBoxCollider2D.size;
        _playerBoxCollider2D.offset = _animBoxCollider2D.offset;
    }
}
