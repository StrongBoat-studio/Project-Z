using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set; }

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

    [field: SerializeField] public EventReference UIButtonClick { get; private set; }
    [field: SerializeField] public EventReference UIButtonHover { get; private set; }

    [field: SerializeField] public EventReference MainTheme { get; private set; }
}
