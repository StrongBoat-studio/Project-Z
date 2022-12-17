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
        if (Input.GetMouseButtonDown(1))
        {
            ItemRegister.Instance.CreateWorldItem(
                ItemRegister.Instance.CreateItem(ItemRegister.Instance.testItemBack),
                Camera.main.ScreenToWorldPoint(Input.mousePosition)
            );
        }
    }
}
