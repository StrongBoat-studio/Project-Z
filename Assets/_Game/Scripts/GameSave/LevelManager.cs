using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelManagerData _levelData;

    private void Awake()
    {
        _levelData.items = new List<LevelManagerData.ItemWorldState>();
        foreach(ItemWorld iw in FindObjectsOfType<ItemWorld>())
        {
            _levelData.items.Add(new LevelManagerData.ItemWorldState(
                iw.GetItem().itemType,
                iw.GetItem().amount,
                iw.transform.position,
                true
            ));
        }
    }

    public LevelManagerData GetLevelData()
    {
        return _levelData;
    }

    public void SetLevelData(LevelManagerData lmd)
    {
        _levelData = lmd;
    }

    public void ExecuteDataLoad()
    {
       LoadWorldItems();
    }

    private void LoadWorldItems()
    { 
        if(GameSaveManager.Instance == null) return;
        if(FindObjectsOfType<ItemWorld>().Length <= 0) return;

        foreach(ItemWorld iw in FindObjectsOfType<ItemWorld>())
        {
            Destroy(iw.gameObject);
        }

        Transform itemWorldContainer = null;
        if(GameObject.FindGameObjectsWithTag("ItemContainer").Length > 0)
            itemWorldContainer = GameObject.FindGameObjectsWithTag("ItemContainer").First().transform;

        foreach(LevelManagerData.ItemWorldState iws in _levelData.items)
        {
            if(iws.load == false) continue;
            if(itemWorldContainer != null)
                ItemRegister.Instance.CreateWorldItem(iws.itemType, iws.amount, iws.position, itemWorldContainer);
            else
            {
                Debug.LogWarning("Can't find game object with itemContainer tag, create one on level's scene.");
                ItemRegister.Instance.CreateWorldItem(iws.itemType, iws.amount, iws.position);
            }
        }
    }
}
