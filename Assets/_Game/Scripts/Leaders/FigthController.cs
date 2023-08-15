using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigthController : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _fight;
    private Transform _targetPlayer;
    public bool canGo = false;

    private const float MOVING_THE_FIGHT_OBJECT = 1.719f;
    private float _movingSideCallculation;
    private float _searchedPositionX;

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

            MovingSideCallculation();

            StartCoroutine(StartFight());
        }
    }

    private void MovingSideCallculation()
    {
        if(_parent.localScale.x==1)
        {
            _movingSideCallculation = MOVING_THE_FIGHT_OBJECT;
        }
        else if(_parent.localScale.x == -1)
        {
            _movingSideCallculation = -MOVING_THE_FIGHT_OBJECT;
        }
    }

    private Vector2 CallculationFightPosition()
    {
        _searchedPositionX = -_movingSideCallculation + _parent.position.x;

        return new Vector2(_searchedPositionX, _parent.position.y);
    }

    public void GoToTarget()
    {
        canGo = true;
        GetComponent<Animator>().SetBool("IsWalk", true);

        if (GetComponentInParent<BorisController>().dotPro > 0)
        {
            _parent.localScale = new Vector2(-_parent.localScale.x, 1);
        }
    }

    private IEnumerator StartFight()
    {
        yield return new WaitForSeconds(1.0f);
        _fight.transform.localScale = _parent.transform.localScale;
        _fight.transform.position = CallculationFightPosition();
        _fight.SetActive(true);
        Destroy(this.gameObject);
    }
}
