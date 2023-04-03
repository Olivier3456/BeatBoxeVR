using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMCheck : MonoBehaviour
{
    public AudioClip musicClip;
    public float bpm;
    public float sampleRate = 44100;
    
    public float bpmThreshold = 0.2f;

    private int bpmWindowSize;
    void Start()
    {
        bpmWindowSize = (int)musicClip.length * 60;
        Debug.Log("bpmWindowSize = " + bpmWindowSize);

        float[] spectrumData = new float[bpmWindowSize];
        musicClip.GetData(spectrumData, 0);

        float maxEnergy = 0;
        int maxIndex = 0;

        for (int i = 0; i < spectrumData.Length; i++)
        {
            float energy = spectrumData[i];
            if (energy > maxEnergy)
            {
                maxEnergy = energy;
                maxIndex = i;
            }
        }

        float frequency = maxIndex * sampleRate / bpmWindowSize;
        bpm = frequency / 2.0f;
    }
}
