using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperActiveController : MonoBehaviour
{
    private JumperStateManager _jumperStateManager;
    private JumperPatrolling _jumperPatrolling;
    private JumperChaseWithAI _jumperChaseWithAI;
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

        _jumperStateManager = this.gameObject.GetComponent<JumperStateManager>();
        _jumperPatrolling = this.gameObject.GetComponent<JumperPatrolling>();
        _jumperChaseWithAI = this.gameObject.GetComponent<JumperChaseWithAI>();
        _sprite = this.gameObject.transform.GetChild(0).gameObject;
        _chaseAlert = this.gameObject.transform.GetChild(0).gameObject;
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
        _jumperStateManager.enabled = isActive;
        _jumperPatrolling.enabled = isActive;
        _jumperChaseWithAI.enabled = isActive;
        _sprite.SetActive(isActive);
        _chaseAlert.SetActive(isActive);
        _lifeBar.SetActive(isActive);
    }
}
