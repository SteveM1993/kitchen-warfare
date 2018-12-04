using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void PlaySound(AudioSource audio, AudioClip sound, bool pitchRandom = false, float minPitch = 1, float maxPitch = 1)
    {
        audio.clip = sound;

        if (pitchRandom)
        {
            audio.pitch = Random.Range(minPitch, maxPitch);
        }

        audio.Play();
    }

    public void InstansiateClip(Vector3 position, AudioClip sound, float time = 2f, bool pitchRandom = false, float minPitch = 1, float maxPitch = 1)
    {
        GameObject clone = new GameObject("One shot audio");
        clone.transform.position = position;
        AudioSource audio = clone.AddComponent<AudioSource>();
        audio.spatialBlend = 1;
        audio.clip = sound;
        audio.Play();
        Destroy(clone, time);
    }
}
