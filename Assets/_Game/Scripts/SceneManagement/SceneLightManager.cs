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
        [Tooltip("Light flicker preset")]
        public LightFlickerPreset preset;

        [HideInInspector] public Queue<float> smoothQueue;
        [HideInInspector] public float lastSum = 0;
        [HideInInspector] public float currentFrequency = 0f;
        [HideInInspector] public float flickerWait = 0f;
        [HideInInspector] public float failSecondTimer = 0f;
        [HideInInspector] public float failWait = 0f;
        [HideInInspector] public bool failed = false;
        [HideInInspector] public float workWait = 0f;
    }

    [SerializeField] private List<LightGroup> _lightGroups;

    private void Start()
    {
        foreach (LightGroup lg in _lightGroups) lg.smoothQueue = new Queue<float>(lg.preset.smoothing);
    }

    private void Update()
    {
        foreach (LightGroup lg in _lightGroups)
        {
            Flicker(lg);
        }
    }

    private void Flicker(LightGroup lg)
    {
        if (lg.failSecondTimer <= 0f)
        {
            lg.failSecondTimer = 1f;

            //Try to make light fail
            if (
                Random.Range(0f, 1f) <= lg.preset.failChance &&
                lg.failed == false &&
                lg.workWait <= 0f
            )
            {
                lg.failed = true;
                lg.failWait = lg.preset.failDuration + Random.Range(-lg.preset.failDurationRandomess, lg.preset.failDurationRandomess);
                lg.lights.ForEach(x => x.intensity = 0f);
            }
        }
        else
        {
            lg.failSecondTimer -= Time.deltaTime;
        }

        //Light failed, decrese fail time, fix after set time
        //Set workTime to ensure at lest minWorkTime of light online
        if (lg.failed == true)
        {
            if (lg.failWait <= 0f)
            {
                lg.failed = false;
                lg.workWait = lg.preset.minWorkTime + Random.Range(-lg.preset.workTimeRandomness, lg.preset.workTimeRandomness);
            }
            else lg.failWait -= Time.deltaTime;
        }
        else
        {
            //Ensure online time to be at least minWorkTime
            if (lg.workWait >= 0) lg.workWait -= Time.deltaTime;

            //Randomize frequency a bit to break up the regularity
            lg.currentFrequency = lg.preset.frequency + Random.Range(-lg.preset.frequencyRandomenss, lg.preset.frequencyRandomenss);

            if (lg.flickerWait <= 0)
            {
                // pop off an item if too big
                while (lg.smoothQueue.Count >= lg.preset.smoothing)
                {
                    lg.lastSum -= lg.smoothQueue.Dequeue();
                }
                lg.flickerWait = 1f / lg.currentFrequency;
            }
            else
            {
                lg.flickerWait -= Time.deltaTime;
            }

            // Generate random new item, calculate new average
            float newVal = Random.Range(lg.preset.minIntensity, lg.preset.maxIntensity);
            lg.smoothQueue.Enqueue(newVal);
            lg.lastSum += newVal;

            // Calculate new smoothed average
            foreach (Light2D l2d in lg.lights) l2d.intensity = lg.lastSum / (float)lg.smoothQueue.Count;
        }
    }
}
