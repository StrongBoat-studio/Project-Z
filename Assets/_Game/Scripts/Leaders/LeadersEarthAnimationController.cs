using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadersEarthAnimationController : MonoBehaviour
{
    private GameObject _boris;
    private GameObject _shimura;

    private bool _canAnimStart = false;
    private bool _canWalk = false;

    //Boris Transparency 
    private float _targetB;
    private float _currentB;
    private int countB = 0;
    private Color32 targetColorB = new Color32(255, 255, 255, 0);

    //Shimura Transparency 
    private float _targetS;
    private float _currentS;
    private int countS = 0;
    private Color32 targetColorS = new Color32(255, 255, 255, 0);

    private void Awake()
    {
        _boris = GameManager.Instance.boris;
        _shimura = GameManager.Instance.shimura;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnGameStateChanged(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.Gameplay && !_canAnimStart)
        {
            return;
        }

        if (newGameState == GameStateManager.GameState.Gameplay && _canAnimStart)
        {
            StartAnim();
            StartCoroutine(StartWalk());
        }

        if (newGameState == GameStateManager.GameState.Dialogue)
        {
            _canAnimStart = true;
            return;
        }
    }
    private void Update()
    {
        if(_canWalk)
        {
            if(_boris != null)
            {
                _boris.transform.position = Vector3.MoveTowards(_boris.transform.position, new Vector3(10.7f, _boris.transform.position.y, 0), Time.deltaTime);
                TransparencyAnimationB();

                if (_boris.transform.position.x == 10.7f)
                {
                    DestroyLeader(_boris);
                }
            }
            
            if (_shimura != null)
            {
                _shimura.transform.position = Vector3.MoveTowards(_shimura.transform.position, new Vector3(10.0f, _shimura.transform.position.y, 0), Time.deltaTime);
                TransparencyAnimationS();

                if (_shimura.transform.position.x == 10.0f)
                {
                    DestroyLeader(_shimura);
                }
            }
        }

        if(_boris == null && _shimura == null)
        {
            Destroy(this);
        }
    }

    private void StartAnim()
    {
        _canAnimStart = false;

        _boris.GetComponentInChildren<Animator>().SetBool("IsSalut", true);
        _shimura.GetComponentInChildren<Animator>().SetBool("IsBow", true);
    }

    private IEnumerator StartWalk()
    {
        yield return new WaitForSeconds(1.5f);
        _canWalk = true;
    }

    private void TransparencyAnimationB()
    {
        if (countB == 10) countB = 0;
        if (countB == 0 && _boris.transform.position.x > 7.0f)
        {
            _targetB = _targetB == 0 ? 1 : 0;
            _currentB = Mathf.MoveTowards(_currentB, _targetB, 0.5f * Time.deltaTime);
            _boris.GetComponentInChildren<SpriteRenderer>().color = Color32.Lerp(_boris.GetComponentInChildren<SpriteRenderer>().color, targetColorB, _currentB);
        }
        countB++;
    }

    private void TransparencyAnimationS()
    {
        if (countS == 10) countS = 0;
        if (countS == 0 && _shimura.transform.position.x > 7.0f)
        {
            _targetS = _targetS == 0 ? 1 : 0;
            _currentS = Mathf.MoveTowards(_currentS, _targetS, 0.5f * Time.deltaTime);
            _shimura.GetComponentInChildren<SpriteRenderer>().color = Color32.Lerp(_shimura.GetComponentInChildren<SpriteRenderer>().color, targetColorS, _currentS);
        }
        countS++;
    }

    private void DestroyLeader(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
