using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory _inventory;

    private void Awake()
    {
        GameManager.Instance.player = transform;
    }

    private void Start()
    {
        _inventory = new Inventory();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ItemRegister.Instance.CreateWorldItem(
                ItemRegister.Instance.CreateItem(ItemRegister.Instance.testItemBack),
                Camera.main.ScreenToWorldPoint(Input.mousePosition)
            );
        }
    }

    ///<summary>
    ///Adda a given amout of given item to the inventory
    ///</summary>
    ///<param name="item"></param>
    ///<param name="quantity"></param>
    public void AddItem(Item item, int quantity)
    {
        _inventory.AddItem(item, quantity);
    }
}
