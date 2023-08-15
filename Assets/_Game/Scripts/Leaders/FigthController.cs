using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigthController : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    private Transform _targetPlayer;
    public bool canGo = false;

    private void Awake()
    {
        _targetPlayer = GameManager.Instance.player.GetChild(3).GetChild(0);
    }

    private void Update()
    {
        if(canGo)
        {
            _parent.position = Vector3.MoveTowards(_parent.position, new Vector2(_targetPlayer.position.x, -1.15f), Time.deltaTime);
        }

        if(_parent.position.x == _targetPlayer.position.x)
        {
            canGo = false;
            GetComponent<Animator>().SetBool("IsWalk", false);
            _parent.localScale = new Vector2(-GameManager.Instance.player.localScale.x, 1);
            StartFight();
        }
    }

    public void GoToTarget()
    {
        canGo = true;
        GetComponent<Animator>().SetBool("IsWalk", true);
    }

    private void StartFight()
    {

    }
}
