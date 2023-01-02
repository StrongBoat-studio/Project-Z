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
        
    }

    public void Heal(float healValue)
    {
        Debug.Log("Healed " + healValue + " HP");
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
}
