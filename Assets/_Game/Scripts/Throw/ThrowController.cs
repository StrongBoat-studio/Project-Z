using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [SerializeField] private GameObject _trash;
    private Trash.TrashType _trashType;
    private GameObject _objectToDestroy;

    private void Awake()
    {
        _trash.SetActive(false);
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
}
