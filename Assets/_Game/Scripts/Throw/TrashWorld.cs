using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class TrashWorld : MonoBehaviour
{
    [SerializeField] private Trash.TrashType _trashType;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _groundlayer;
    [SerializeField] private int _throwForce;

    private Rigidbody2D _rigidbody2D;
    public bool isGrounded=true;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = TrashRegister.Instance.trashes.Find(x => x.trashType == _trashType).sprite;
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        if(!isGrounded)
        {
            //_rigidbody2D.AddForce(new Vector2(GetComponentInParent<Transform>().localScale.x * _throwForce, 1*_throwForce/4), ForceMode2D.Impulse);
            _rigidbody2D.velocity = new Vector2(GetComponentInParent<Transform>().localScale.x * _throwForce, 1 * _throwForce / 4);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCapsule(_groundCheck.transform.position, new Vector2(0.3f, 0.05f), CapsuleDirection2D.Horizontal, 0, _groundlayer);
        Debug.Log(isGrounded);

        if(!isGrounded)
        {
            _rigidbody2D.gravityScale = 2.0f;
        }
        else
        {
            _rigidbody2D.gravityScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponentInChildren<ThrowController>()!=null && isGrounded)
        {
            collision.gameObject.GetComponentInChildren<ThrowController>().GetTrash(_trashType, this.gameObject);
        }
    }

    public void SetTrashType(Trash.TrashType _trashType)
    {
        this._trashType = _trashType;
    }
}
