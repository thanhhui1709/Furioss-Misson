using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine.Audio;

public class PoolAudioPlayer: MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        AdjustAudioSource();
    }

    public void PlayAudioClip(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        StartCoroutine(ReturnAfterPlay(clip.length));
    }

    private IEnumerator ReturnAfterPlay(float duration)
    {
        yield return new WaitForSeconds(duration);
        ObjectPoolManager.ReturnObject(gameObject);
    }
    private void AdjustAudioSource()
    {   if (audioSource == null) return;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 20f;
        audioSource.dopplerLevel = 0.5f;
    }
}
