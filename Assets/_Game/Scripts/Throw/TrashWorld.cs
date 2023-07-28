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
    private float _throwDirection;
    public bool isGrounded = true;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = TrashRegister.Instance.trashes.Find(x => x.trashType == _trashType).sprite;
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCapsule(_groundCheck.transform.position, new Vector2(0.3f, 0.05f), CapsuleDirection2D.Horizontal, 0, _groundlayer);
        Debug.Log(isGrounded);

        if (isGrounded)
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            this.transform.Rotate(0, 0, _throwDirection * 10f);
            this.gameObject.GetComponentInChildren<Transform>().Rotate(0, 0, _throwDirection * 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponentInChildren<ThrowController>()!=null && isGrounded)
        {
            collision.gameObject.GetComponentInChildren<ThrowController>().GetTrash(_trashType, this.gameObject);    
        }

        if(collision.gameObject.tag=="Ground")
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        }
    }

    public void SetTrashType(Trash.TrashType _trashType)
    {
        this._trashType = _trashType;
    }

    public void Throw(float _throwDirection)
    {
        if (!isGrounded)
        {
            _rigidbody2D.AddForce(new Vector2(-_throwDirection * _throwForce, 1), ForceMode2D.Impulse);
            this._throwDirection = -_throwDirection;
        }
    }
}
