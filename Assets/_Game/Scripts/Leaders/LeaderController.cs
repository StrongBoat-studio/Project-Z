using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderController : MonoBehaviour
{
    [SerializeField] List<string> _questsNameWhenShowBoris;
    [SerializeField] List<string> _questsNameWhenShowShimura;

    [SerializeField] GameObject _boris;
    [SerializeField] GameObject _shimura;

    public bool canShowBoris = false;
    public bool canShowShimura = false;

    private void Awake()
    {
        GameManager.Instance.boris = _boris;
        GameManager.Instance.shimura = _shimura;
    }
    void Update()
    {
        if(_boris!=null)
        {
            Show(_questsNameWhenShowBoris, _boris);

            canShowBoris = _boris.activeSelf;
        }
        
        if(_shimura!=null)
        {
            Show(_questsNameWhenShowShimura, _shimura);

            canShowShimura = _shimura.activeSelf;
        }
    }

    private void Show(List<string> _questsName, GameObject objectToShow)
    {
        foreach (string questName in _questsName)
        {
            if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == questName)
            {
                objectToShow.SetActive(true);
                return;
            }
        }
        objectToShow.SetActive(false);
    }
}
