using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class TrashWorld : MonoBehaviour
{
    [SerializeField] Trash.TrashType _trashType;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = TrashRegister.Instance.trashes.Find(x => x.trashType == _trashType).sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<WalkerStateManager>()!=null)
        {
            Debug.Log("Walker in trash");
        }
    }
}
