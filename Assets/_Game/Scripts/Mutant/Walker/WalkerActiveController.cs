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

    private List<string> _questsSetActiveTrue = new List<string>();

    private void Awake()
    {
        _questsSetActiveTrue.Add("Approach the Greenhouse");
        _questsSetActiveTrue.Add("Talk to Boris");
        _questsSetActiveTrue.Add("Find the source of the gunshot sound");
        _questsSetActiveTrue.Add("Search the body");
        _questsSetActiveTrue.Add("Argue with Boris");
        _questsSetActiveTrue.Add("Find: The gunpowder");
        _questsSetActiveTrue.Add("Find: The Test Vial");
        _questsSetActiveTrue.Add("Find: The Chernobyl herb");
        _questsSetActiveTrue.Add("Find: The Isolated Saliva of Subject “0”");
        _questsSetActiveTrue.Add("Return to the Lab");
        _questsSetActiveTrue.Add("Create the Antidote");

        _walkerStateManager = this.gameObject.GetComponent<WalkerStateManager>();
        _walkerPatrolling = this.gameObject.GetComponent<WalkerPatrolling>();
        _walkerChaseWithAI = this.gameObject.GetComponent<WalkerChaseWithAI>();
        _sprite = this.gameObject.transform.GetChild(0).gameObject;
        _chaseAlert = this.gameObject.transform.GetChild(1).gameObject;
        _lifeBar = this.gameObject.transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        if (_questsSetActiveTrue.Contains(QuestLineManager.Instance.Quests[0].Tasks[0].Title))
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
