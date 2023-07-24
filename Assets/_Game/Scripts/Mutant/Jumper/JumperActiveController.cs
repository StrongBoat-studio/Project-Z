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

    private void Awake()
    {
        _jumperStateManager = this.gameObject.GetComponent<JumperStateManager>();
        _jumperPatrolling = this.gameObject.GetComponent<JumperPatrolling>();
        _jumperChaseWithAI = this.gameObject.GetComponent<JumperChaseWithAI>();
        _sprite = this.gameObject.transform.GetChild(0).gameObject;
        _chaseAlert = this.gameObject.transform.GetChild(0).gameObject;
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
        _jumperStateManager.enabled = isActive;
        _jumperPatrolling.enabled = isActive;
        _jumperChaseWithAI.enabled = isActive;
        _sprite.SetActive(isActive);
        _chaseAlert.SetActive(isActive);
        _lifeBar.SetActive(isActive);
    }
}
