using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AlarmLight : MonoBehaviour
{
    [SerializeField] List<Light2D> _lightsThatChangeColorForAlarm;

    public void AlarmStart()
    {
        foreach (Light2D light in _lightsThatChangeColorForAlarm)
        {
            light.color = new Color32(255, 0, 0, 255);
        }
    }

    public void AlarmStop()
    {
        foreach (Light2D light in _lightsThatChangeColorForAlarm)
        {
            light.color = new Color32(255, 255, 255, 255);
        }
    }
}
