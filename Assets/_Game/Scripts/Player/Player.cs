using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory _inventory;

    private void Start()
    {
        _inventory = new Inventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _inventory.AddItem(ItemRegister.Instance.CreateItem(ItemRegister.Instance.testItem), 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _inventory.RemoveItem(ItemRegister.Instance.CreateItem(ItemRegister.Instance.testItem), 1);
        }
    }
}
