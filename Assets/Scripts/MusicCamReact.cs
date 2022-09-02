using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCamReact : MonoBehaviour
{
    public AudioSource audioSource;
    public Camera cam;
    public float updateStep = 0.01f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;
    public float offset = 0.5f;
    public float damping;
    public float clipLoudness;
    public float intensity;
    private float[] clipSampleData;

    public float intensityFactor;
    public float minIntensity;
    public float maxIntensity;

    private void Awake() {
        clipSampleData = new float[sampleDataLength];
        cam = GetComponent<Camera>();
        intensity = 0f;
    }

    private void Update() {
        if (!audioSource.isPlaying) {
            return;
        }
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep) {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
            clipLoudness = 0f;
            foreach (var sample in clipSampleData) {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength;
            clipLoudness *= intensityFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, minIntensity, maxIntensity);
            if (clipLoudness < intensity - damping*Time.deltaTime) {
                intensity -= damping*Time.deltaTime;
            } else {
                intensity = clipLoudness;
            }
            cam.orthographicSize = intensity;
        }
    }
}
