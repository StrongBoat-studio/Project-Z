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

    //UI
    [field: SerializeField]
    public EventReference UIButtonClick { get; private set; }

    [field: SerializeField]
    public EventReference UIButtonHover { get; private set; }

    [field: SerializeField]
    public EventReference QuestFinish { get; private set; }

    [field: SerializeField]
    public EventReference JournalLifeMonitor { get; private set; }

    //Ambience
    [field: SerializeField]
    public EventReference CCTVNoise { get; private set; }

    [field: SerializeField]
    public EventReference Alarm { get; private set; }

    //Music
    [field: SerializeField]
    public EventReference MainTheme { get; private set; }

    //Player
    [field: SerializeField]
    public EventReference Walk { get; private set; }

    [field: SerializeField]
    public EventReference WalkCarpet { get; private set; }

    [field: SerializeField]
    public EventReference WalkSteel { get; private set; }

    [field: SerializeField]
    public EventReference NoStaminaGasp { get; private set; }

    [field: SerializeField]
    public EventReference Jump { get; private set; }

    [field: SerializeField]
    public EventReference JumpLanding { get; private set; }

    [field: SerializeField]
    public EventReference FlashlightOn { get; private set; }

    [field: SerializeField]
    public EventReference FlashlightOff { get; private set; }

    [field: SerializeField]
    public EventReference LockerHide { get; private set; }

    //Weapons
    [field: SerializeField]
    public EventReference Shot { get; private set; }

    [field: SerializeField]
    public EventReference NoAmmo { get; private set; }

    [field: SerializeField]
    public EventReference GunReaload { get; private set; }

    [field: SerializeField]
    public EventReference KnifeHit { get; private set; }

    //Items
    [field: SerializeField]
    public EventReference ItemPickup { get; private set; }

    //Mutants
    [field: SerializeField]
    public EventReference MutantGrowl { get; private set; }

    [field: SerializeField]
    public EventReference MutantIdle { get; private set; }
}
