using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class UI_LifeMonitor : MonoBehaviour
{
    private EventInstance _audio;

    private void OnEnable()
    {
        StartAudio();
    }

    private void OnDisable()
    {
        StopAudio();
    }

    private void OnDestroy()
    {
        StopAudio();
    }

    public void StartAudio()
    {
        if (AudioManager.Instance != null && FMODEvents.Instance != null)
        {
            _audio = AudioManager.Instance.CreateInstance(FMODEvents.Instance.JournalLifeMonitor);
            _audio.start();
        }
    }

    public void StopAudio()
    {
        _audio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _audio.release();
    }
}
