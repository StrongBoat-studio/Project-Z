using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _bloodPS;
    [SerializeField] private GameObject _dustPS;
    [SerializeField] private BossStateManager _bossStateManager;

    private bool _isAttack = false; 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isAttack==false)
        {
            Debug.Log("collison with player");
            _isAttack = true;

            if(_bossStateManager.nextAttackId==3)
            {
                _dustPS.transform.position = GetComponentInParent<Transform>().position;
                _dustPS.SetActive(true);
                TakeDamage();
            }
            else
            {
                _bloodPS.transform.position = GetComponentInParent<Transform>().position;
                _bloodPS.SetActive(true);
                TakeDamage();
            } 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isAttack = false;
            _bloodPS.SetActive(false);
            _dustPS.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        if (GameManager.Instance.Player == null) return;

        GameManager.Instance.Player.TakeDamage(Random.Range(8, 15));
    }
}
