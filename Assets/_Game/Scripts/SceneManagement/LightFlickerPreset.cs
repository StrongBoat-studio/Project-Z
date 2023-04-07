using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Light Flicker Preset", menuName = "Light flicering/Preset")]
public class LightFlickerPreset : ScriptableObject
{
    [Tooltip("Minimum intensity that light can achieve")]
    public float minIntensity = 0f;
    [Tooltip("Maximum intensity that light can achieve")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth random filcerings")]
    [Range(1, 50)] public int smoothing = 5;
    [Tooltip("Frequency of the light flicker")]
    public float frequency = 1f;
    [Tooltip("Base frequency randomness")]
    public float frequencyRandomenss = 1f;
    [Tooltip("Light fail chance (each second)")]
    [Range(0f, 1f)] public float failChance = 0f;
    [Tooltip("Light fail duration")]
    [Range(0f, 10f)] public float failDuration = .25f;
    [Tooltip("Variation of the fail duration, keep it below failDuration value")]
    [Range(0f, 10f)] public float failDurationRandomess = .25f;
    [Tooltip("Minimum working time after fail")]
    public float minWorkTime = 1f;
    [Tooltip("Variation of the work time, keep it below minWorkTime value")]
    public float workTimeRandomness = 0f;
}
