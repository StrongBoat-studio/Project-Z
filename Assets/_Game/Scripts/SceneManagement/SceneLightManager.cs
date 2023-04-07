using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

// Modified to work with 2D light and light groups

/// <summary>
/// Component the makes the lights flicker
/// You can make light groups to make lights flicker at the same time
/// </summary>
public class SceneLightManager : MonoBehaviour
{
    [System.Serializable]
    private class LightGroup
    {
        [Tooltip("Lights to be affected by flickering")]
        public List<Light2D> lights;
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

        public Queue<float> smoothQueue;
        [HideInInspector] public float lastSum = 0;
        [HideInInspector] public float currentFrequency = 0f;
        [HideInInspector] public float currentWait = 0f;
    }

    [SerializeField] private List<LightGroup> _lightGroups;

    void Start()
    {
        foreach (LightGroup lg in _lightGroups) lg.smoothQueue = new Queue<float>(lg.smoothing);
    }

    void Update()
    {
        foreach (LightGroup lg in _lightGroups)
        {
            Flicker(lg);
        }
    }

    private void Flicker(LightGroup lg)
    {
        lg.currentFrequency = lg.frequency + Random.Range(-lg.frequencyRandomenss, lg.frequencyRandomenss);

        if (lg.currentWait <= 0)
        {
            // pop off an item if too big
            while (lg.smoothQueue.Count >= lg.smoothing)
            {
                lg.lastSum -= lg.smoothQueue.Dequeue();
            }
            lg.currentWait = 1f/lg.currentFrequency;
        }
        else
        {
            lg.currentWait -= Time.deltaTime;
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(lg.minIntensity, lg.maxIntensity);
        lg.smoothQueue.Enqueue(newVal);
        lg.lastSum += newVal;

        // Calculate new smoothed average
        foreach (Light2D l2d in lg.lights) l2d.intensity = lg.lastSum / (float)lg.smoothQueue.Count;
    }
}
