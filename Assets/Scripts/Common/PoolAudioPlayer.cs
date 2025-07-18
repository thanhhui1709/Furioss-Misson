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
}
