using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [SerializeField] private GameObject _trash;
    [SerializeField] private GameObject _trashPrefab;
    [SerializeField] private int _throwForce;
    private Trash.TrashType _trashType;
    private GameObject _objectToDestroy;

    public Vector2 trashPosition = new Vector2(0.21f, -0.18f);

    private void Awake()
    {
        _trash.SetActive(false);
    }
    private void Update()
    {
        if (trashPosition == Vector2.zero)
        {
            trashPosition = new Vector2(0.21f, -0.18f);
        }
        _trash.transform.localPosition = trashPosition;
    }

    public void GetTrash(Trash.TrashType trashTypeCollision, GameObject objectToDestroy)
    {
        GetComponentInParent<WalkerStateManager>().isPickUp = true;
        GetComponentInParent<WalkerPatrolling>().enabled = false;
        GetComponentInParent<WalkerChaseWithAI>().enabled = false;
        GetComponentInParent<Animator>().SetBool("IsPickUp", true);

        _trashType = trashTypeCollision;
        _objectToDestroy = objectToDestroy;
    }

    public void SetTrash()
    {
        _trash.GetComponent<SpriteRenderer>().sprite = TrashRegister.Instance.trashes.Find(x => x.trashType == _trashType).sprite;
        Destroy(_objectToDestroy);
        _trash.SetActive(true);
    }

    public void PickUpFalse()
    {
        GetComponent<Animator>().SetBool("IsPickUp", false);
        GetComponent<Animator>().SetBool("IsThrow", true);
    }

    public void StartThrow()
    {
        _trash.SetActive(false);
        _trashPrefab.GetComponent<TrashWorld>().SetTrashType(_trashType);
        _trashPrefab.GetComponent<TrashWorld>().isGrounded = false;
        Instantiate(_trashPrefab, _trash.transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Trashes").transform);

    }

    public void StopThrow()
    {
        GetComponent<Animator>().SetBool("IsThrow", false);
        GetComponentInParent<WalkerStateManager>().isPickUp = false;
        GetComponentInParent<WalkerPatrolling>().enabled = true;
        GetComponentInParent<WalkerChaseWithAI>().enabled = true;
    }
}
