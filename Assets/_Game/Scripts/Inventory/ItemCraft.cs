using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCraft : MonoBehaviour
{
    public static ItemCraft Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [System.Serializable]
    public struct Recipe
    {
        public Item.ItemType itemBase;
        public Item.ItemType itemDrop;
        public Item.ItemType itemResult;
    }

    public List<Recipe> recipes;

    ///<summary>
    ///Crafts an Item based on provided slots with items.
    ///Consumes one copy of provided items from slots if crafting was successful
    ///</summary>
    ///<param name="slotBase">Slot on which the other slot is dropped</param>
    ///<param name="slotDrop">Slot which is dropped on another slot</param>
    public void TryCraft(UI_InventorySlot slotBase, UI_InventorySlot slotDrop)
    {
        int idRecipe = recipes.FindIndex(x => 
            x.itemBase == slotBase.GetItem().itemType && 
            x.itemDrop == slotDrop.GetItem().itemType
        );

        if(idRecipe < 0) return;

        Item crafted = ItemRegister.Instance.GetNewItem(recipes[idRecipe].itemResult);
        crafted.amount = 1;
        slotBase.GetItem().Use?.Invoke(slotBase.GetItem(), 1);
        slotDrop.GetItem().Use?.Invoke(slotDrop.GetItem(), 1);
        GameManager.Instance.player.GetComponent<Player>().GetInventory().AddItem(crafted);
    }
}
