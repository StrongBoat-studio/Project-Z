using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManagerData
{
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

    public SceneRegister.Scenes sceneIndex;
    public List<ItemWorldState> items;
}
