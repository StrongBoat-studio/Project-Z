using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool showBoris = true;
    public bool showShimura = true;

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

    public Transform player;
    public Player Player;
    public Movement movement;
    public GameObject boris;
    public GameObject shimura;

    public void Reset()
    {
        player = null;
    }

    public bool ShowBoris()
    {
        if (showBoris)
            return true;
        else
            return false;
    }

    public bool ShowShimura()
    {
        if (showShimura)
            return true;
        else
            return false;
    }
} 
