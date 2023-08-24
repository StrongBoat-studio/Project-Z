using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class MovementAudio : MonoBehaviour
{
    public void Play()
    {
        AudioManager.Instance?.PlayOneShotFromPath(GetPath(), transform.position);
    }

    private string GetPath()
    {
        //Play default footstep sound if AudioManager Instance is null
        if(AudioManager.Instance == null) return "event:/SFX/Player/Walk";

        int idx = AudioManager.Instance.SceneFootstep.FindIndex(x => (int)x.scene == GameSaveManager.Instance?.currentSave.locationIndex);
        if(idx == -1)
        {
            return "event:/SFX/Player/Walk";
        }
        
        switch(AudioManager.Instance?.SceneFootstep[idx].type)
        {
            case AudioManager.FootstepType.Normal:
                return "event:/SFX/Player/Walk";
            case AudioManager.FootstepType.Carpet:
                return "event:/SFX/Player/WalkCarpet";
            case AudioManager.FootstepType.Steel:
                return "event:/SFX/Player/WalkSteel";
            default:
                return "event:/SFX/Player/Walk";
        }
    }
}
