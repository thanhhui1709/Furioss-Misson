using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class ThemeAudio : MonoBehaviour
{
    public static ThemeAudio Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    public AudioClip themeAudioClip;
    private AudioSource audioSource;
    private void OnEnable()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (themeAudioClip != null)
        {
            GameEvent.instance.onStageStart.AddListener(() => PlayThemeAudio(themeAudioClip,0.6f));
        }
    }
    private void OnDisable()
    {
        if (themeAudioClip != null)
        {
            GameEvent.instance.onStageStart.RemoveListener(() => PlayThemeAudio(themeAudioClip,0,6f));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayThemeAudio(AudioClip audioClip,float volume, float time = 5f)
    {
        StartCoroutine(PlayThemeAudioCoroutine(audioClip, volume, time));
    }
    IEnumerator PlayThemeAudioCoroutine(AudioClip clip, float volume,float time)
    {
        yield return new WaitForSeconds(time);
        if (audioSource != null && themeAudioClip != null)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.Play();
        }

    }
    public void StopThemeAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void PauseThemeAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
    public void UnpauseThemeAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
    public void SetVolumeByTime(float volume, float time)
    {
        StartCoroutine(SetVolumeByTimeCoroutine(volume, time));
    }
    IEnumerator SetVolumeByTimeCoroutine(float volume, float time)
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                audioSource.volume = Mathf.Lerp(startVolume, volume, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = volume; // Ensure final volume is set
        }
    }
}
