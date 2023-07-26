using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashRegister : MonoBehaviour
{
    public static TrashRegister Instance { get; private set; }

    public GameObject worldItemPrefab;
    public List<Trash> trashes = new List<Trash>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


}
