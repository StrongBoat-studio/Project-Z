using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public enum VolumeBus
    {
        Master,
        Music,
        SFX,
        Ambience
    }

    public static AudioManager Instance { get; private set; }

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

        eventInstances = new List<EventInstance>();

        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    // [Header("Volume")]
    // [Range(0, 1)]
    // public float masterVolume = 1;
    // [Range(0, 1)]
    // public float musicVolume = 1;
    // [Range(0, 1)]
    // public float ambienceVolume = 1;
    // [Range(0, 1)]
    // public float sfxVolume = 1;

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _ambienceBus;
    private Bus _sfxBus;

    private List<EventInstance> eventInstances;
    private EventInstance _mainThemeEventInstance;

    public void UpdateVolume(VolumeBus bus, float value)
    {
        switch(bus)
        {
            case VolumeBus.Master:
                _masterBus.setVolume(value);
                break;
            case VolumeBus.Music:
                _musicBus.setVolume(value);
                break;
            case VolumeBus.SFX:
                _sfxBus.setVolume(value);
                break;
            case VolumeBus.Ambience:
                _ambienceBus.setVolume(value);
                break;
        }
    }

    public void PlayOneShot(EventReference eventRef, Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventRef, position);
    }

    public void InitializeMainTheme(EventReference eventReference)
    {
        _mainThemeEventInstance = CreateInstance(eventReference);
        _mainThemeEventInstance.start();
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
}
