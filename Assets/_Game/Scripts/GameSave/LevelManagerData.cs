using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManagerData
{
    public enum MutantType 
    {
        Walker,
        Jumper,
        Heavy
    }

    [System.Serializable]
    public struct ItemWorldState
    {
        public ItemWorldState(Item.ItemType itemType, int amount, Vector3 position, bool load)
        {
            this.itemType = itemType;
            this.amount = amount;
            this.position = position;
            this.load = load; 
        }

        public Item.ItemType itemType;
        public int amount;
        public Vector3 position;
        public bool load;
    }

    [System.Serializable]
    public struct MutantState
    {
        public Vector3 position;
        public MutantType type;
        public int hp;
        public bool load;
    }

    [System.Serializable]
    public struct NPCState
    {
        public NPCState(int id, bool load)
        {
            this.id = id;
            this.load = load;
        }

        public int id;
        public bool load;
    }

    public SceneRegister.Scenes sceneIndex;
    public List<ItemWorldState> items;
    public List<MutantState> mutants;
    public List<NPCState> npcs;
}
