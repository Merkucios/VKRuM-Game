using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;

    public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
    {
        _audioSource.clip = clip;
        settings.ApplyTo(_audioSource);
        _audioSource.transform.position = position;
        _audioSource.loop = hasToLoop;
        _audioSource.time = 0f; 
        _audioSource.Play();

        if (!hasToLoop)
        {
            StartCoroutine(FinishedPlaying(clip.length));
        }
    }

    public void FadeMusicIn(AudioClip musicClip, AudioConfigurationSO settings, float duration, float startTime = 0f)
    {
        PlayAudioClip(musicClip, settings, true);
        _audioSource.volume = 0f;

        if (startTime <= _audioSource.clip.length)
            _audioSource.time = startTime;

        StartCoroutine(FadeVolume(_audioSource, 0f, settings.Volume, duration));
    }

    public float FadeMusicOut(float duration)
    {
        StartCoroutine(FadeVolume(_audioSource, _audioSource.volume, 0f, duration, OnFadeOutComplete));

        return _audioSource.time;
    }

    private IEnumerator FadeVolume(AudioSource audioSource, float startVolume, float targetVolume, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        onComplete?.Invoke();
    }

    private void OnFadeOutComplete()
    {
        NotifyBeingDone();
    }

    public AudioClip GetClip()
    {
        return _audioSource.clip;
    }

    public void Resume()
    {
        _audioSource.Play();
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void Finish()
    {
        if (_audioSource.loop)
        {
            _audioSource.loop = false;
            float timeRemaining = _audioSource.clip.length - _audioSource.time;
            StartCoroutine(FinishedPlaying(timeRemaining));
        }
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return _audioSource.loop;
    }

    private IEnumerator FinishedPlaying(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        NotifyBeingDone();
    }

    private void NotifyBeingDone()
    {
        OnSoundFinishedPlaying?.Invoke(this);
    }
}
