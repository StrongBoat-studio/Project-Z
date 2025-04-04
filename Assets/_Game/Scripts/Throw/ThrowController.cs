using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [SerializeField] private GameObject _trash;
    [SerializeField] private Transform _walker;
    [SerializeField] private GameObject _trashPrefab;
    [SerializeField] private int _throwForce;
    private Trash.TrashType _trashType;
    private GameObject _objectToDestroy;
    private GameObject _newTrash;

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

        if(GetComponentInParent<Animator>().GetBool("IsPickUp")==true)
        {
            GetComponent<Animator>().SetBool("IsAttack1", false);
            GetComponent<Animator>().SetBool("InAttack2", false);
            GetComponent<Animator>().SetBool("IsStanding", false);
        }
    }

    public void GetTrash(Trash.TrashType trashTypeCollision, GameObject objectToDestroy)
    {
        if (GetComponentInParent<WalkerStateManager>().GetWalkerState() == GetComponentInParent<WalkerStateManager>().ChaseState)
        {
            GetComponentInParent<WalkerStateManager>().isPickUp = true;
            GetComponentInParent<WalkerPatrolling>().enabled = false;
            GetComponentInParent<WalkerChaseWithAI>().enabled = false;
            GetComponentInParent<Animator>().SetBool("IsPickUp", true);

            _trashType = trashTypeCollision;
            _objectToDestroy = objectToDestroy;
        }
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

        _newTrash = Instantiate(_trashPrefab, _trash.transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Trashes").transform);
        _newTrash.GetComponent<TrashWorld>().Throw(_walker.localScale.x);
    }

    public void StopThrow()
    {
        GetComponent<Animator>().SetBool("IsThrow", false);
        GetComponentInParent<WalkerStateManager>().isPickUp = false;
        GetComponentInParent<WalkerPatrolling>().enabled = true;
        GetComponentInParent<WalkerChaseWithAI>().enabled = true;
    }
}
