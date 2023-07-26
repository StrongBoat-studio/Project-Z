using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerActiveController : MonoBehaviour
{
    private WalkerStateManager _walkerStateManager;
    private WalkerPatrolling _walkerPatrolling;
    private WalkerChaseWithAI _walkerChaseWithAI;
    private GameObject _sprite;
    private GameObject _chaseAlert;
    private GameObject _lifeBar;

    private void Awake()
    {
        _walkerStateManager = this.gameObject.GetComponent<WalkerStateManager>();
        _walkerPatrolling = this.gameObject.GetComponent<WalkerPatrolling>();
        _walkerChaseWithAI = this.gameObject.GetComponent<WalkerChaseWithAI>();
        _sprite = this.gameObject.transform.GetChild(0).gameObject;
        _chaseAlert = this.gameObject.transform.GetChild(1).gameObject;
        _lifeBar = this.gameObject.transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == "Approach the Greenhouse")
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    private void SetActive(bool isActive)
    {
        if (GetComponent<WalkerStateManager>().isPickUp) return;

        _walkerStateManager.enabled = isActive;
        _walkerPatrolling.enabled = isActive;
        _walkerChaseWithAI.enabled = isActive;
        _sprite.SetActive(isActive);
        _chaseAlert.SetActive(isActive);
        _lifeBar.SetActive(isActive);
    }
}
