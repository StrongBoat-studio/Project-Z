using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public struct MutantState
    {
        public Transform mutant;
        public bool load;
    }

    [System.Serializable]
    public struct ItemWorldState
    {
        public Transform item;
        public bool load;
    }

    [System.Serializable]
    public struct NPCState
    {
        public Transform npc;
        public bool load;
    }

    public int sceneIndex;
    public MutantState[] mutants;
    public ItemWorldState[] items;
    public NPCState[] npcs;
}
