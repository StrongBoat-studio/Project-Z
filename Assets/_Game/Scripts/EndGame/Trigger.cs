using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private bool _isTriger = false;
    [SerializeField] private GameObject _jumper;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player" && _isTriger==false)
        {
            _isTriger = true;
            GameManager.Instance.movement.canMove = false;
            _jumper.SetActive(true);

            Destroy(this.gameObject);
        }
    }
}
