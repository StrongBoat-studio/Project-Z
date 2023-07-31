using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderController : MonoBehaviour
{
    [SerializeField] List<string> _questsNameWhenShowBoris;
    [SerializeField] List<string> _questsNameWhenShowShimura;

    [SerializeField] GameObject _boris;
    [SerializeField] GameObject _shimura;

    // Update is called once per frame
    void Update()
    {
        Show(_questsNameWhenShowBoris, _boris);
        Show(_questsNameWhenShowShimura, _shimura);
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
