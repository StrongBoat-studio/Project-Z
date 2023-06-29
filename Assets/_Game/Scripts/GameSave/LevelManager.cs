using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelManagerData _levelData;
    [SerializeField] private Transform _walkerPrefab;
    [SerializeField] private Transform _jumperPrefab;

    private void Awake()
    {
        if (GameSaveManager.Instance != null)
        {
            int idx = GameSaveManager.Instance.currentSave.levelManagerDatas.FindIndex(
                x => x.sceneIndex == _levelData.sceneIndex
            );

            if (idx != -1)
            {
                _levelData = GameSaveManager.Instance.currentSave.levelManagerDatas[idx];
                ExecuteDataLoad();
            }
            else
            {
                //Save items
                SaveItems();
                //Save npcs
                SaveNPCS();
                //Save mutants
                SaveMutants();
            }
        }
    }

    public LevelManagerData GetLevelData()
    {
        //Get level data with updated mutants
        SaveMutants();
        return _levelData;
    }

    public void SetLevelData(LevelManagerData lmd)
    {
        //Load data only if there isn't any problem
        if (_levelData.items.Count == lmd.items.Count)
            _levelData.items = lmd.items;

        if (_levelData.npcs.Count == lmd.npcs.Count)
            _levelData.npcs = lmd.npcs;

        if (_levelData.mutants.Count == lmd.mutants.Count)
            _levelData.mutants = lmd.mutants;

        _levelData.sceneIndex = lmd.sceneIndex;
    }

    public void ExecuteDataLoad()
    {
        LoadWorldItems();
        LoadNPCs();
        LoadMutants();
    }

    private void LoadWorldItems()
    {
        if (GameSaveManager.Instance == null) return;
        if (FindObjectsOfType<ItemWorld>().Length <= 0) return;

        foreach (ItemWorld iw in FindObjectsOfType<ItemWorld>())
        {
            Destroy(iw.gameObject);
        }

        Transform itemWorldContainer = null;
        if (GameObject.FindGameObjectsWithTag("ItemContainer").Length > 0)
            itemWorldContainer = GameObject.FindGameObjectsWithTag("ItemContainer").First().transform;

        foreach (LevelManagerData.ItemWorldState iws in _levelData.items)
        {
            if (iws.load == false) continue;
            if (itemWorldContainer != null)
                ItemRegister.Instance.CreateWorldItem(iws.itemType, iws.amount, iws.position, itemWorldContainer);
            else
            {
                Debug.LogWarning("Can't find game object with itemContainer tag, create one on level's scene.");
                ItemRegister.Instance.CreateWorldItem(iws.itemType, iws.amount, iws.position);
            }
        }
    }

    private void LoadNPCs()
    {
        if (GameSaveManager.Instance == null) return;
        if (FindObjectsOfType<DialogueHolder>().Length <= 0) return;

        foreach (DialogueHolder dh in FindObjectsOfType<DialogueHolder>())
        {
            int lmIDX = GameSaveManager.Instance.currentSave.levelManagerDatas.FindIndex(
                x => (int)x.sceneIndex == GameSaveManager.Instance.currentSave.locationIndex
            );
            if (lmIDX == -1) continue;
            int npcStateIDX = GameSaveManager.Instance.currentSave.levelManagerDatas[lmIDX].npcs.FindIndex(x => x.id == dh.npcSceneID);

            if (npcStateIDX == -1) continue;
            LevelManagerData.NPCState npcState = GameSaveManager.Instance.currentSave.levelManagerDatas[lmIDX].npcs[npcStateIDX];

            if (npcState.load == false)
                Destroy(dh.gameObject);
        }
    }

    private void LoadMutants()
    {
        if (GameSaveManager.Instance == null) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) return;

        foreach (GameObject mutant in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(mutant);
        }

        Transform mutantContainer = null;
        if (GameObject.FindGameObjectsWithTag("MutantContainer").Length > 0)
            mutantContainer = GameObject.FindGameObjectsWithTag("MutantContainer").First().transform;

        foreach (LevelManagerData.MutantState ms in _levelData.mutants)
        {
            if (ms.load == false) continue;

            if (mutantContainer != null)
            {
                Transform mutant = null;
                switch (ms.type)
                {
                    case LevelManagerData.MutantType.Walker:
                        mutant = Instantiate(_walkerPrefab, ms.position, Quaternion.identity, mutantContainer);
                        break;
                    case LevelManagerData.MutantType.Jumper:
                        mutant = Instantiate(_jumperPrefab, ms.position, Quaternion.identity, mutantContainer);
                        break;
                }

                IMutantInit imi = mutant.GetComponent<IMutantInit>();
                imi.SetLife(ms.hp);
                imi.SetDefaultState();
                imi.SetPosition(ms.position);
                imi.UpdateInit();
            }
            else
            {
                Debug.LogWarning("Can't find game object with mutantContainer tag, create one on level's scene.");
            }
        }
    }

    public void SaveItems()
    {
        _levelData.items = new List<LevelManagerData.ItemWorldState>();
        foreach (ItemWorld iw in FindObjectsOfType<ItemWorld>())
        {
            _levelData.items.Add(new LevelManagerData.ItemWorldState(
                iw.GetItem().itemType,
                iw.GetItem().amount,
                iw.transform.position,
                true
            ));
        }
    }

    public void SaveNPCS()
    {
        _levelData.npcs = new List<LevelManagerData.NPCState>();
        foreach (DialogueHolder dh in FindObjectsOfType<DialogueHolder>())
        {
            _levelData.npcs.Add(new LevelManagerData.NPCState(
               dh.npcSceneID,
               true
            ));
        }
    }

    //Temporary, update save as the walker moves, loses helth, etc.
    public void SaveMutants()
    {
        if (GameSaveManager.Instance == null) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) return;

        _levelData.mutants = new List<LevelManagerData.MutantState>();
        foreach (GameObject mutant in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            IMutantInit imi = mutant.GetComponent<IMutantInit>();
            _levelData.mutants.Add(new LevelManagerData.MutantState(
                imi.GetPosition(),
                imi.GetMutantType(),
                imi.GetLife(),
                imi.GetLife() <= 0 ? false : true
            ));
        }
    }
}
