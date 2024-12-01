using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("SoundEmitters pool")]
    [SerializeField] private SoundEmitterPoolSO _pool = default;
    [SerializeField] private int _initialSize = 5;
    
    [SerializeField] private AudioEventSO _SFXEventChannel = default;
    [SerializeField] private AudioEventSO _musicEventChannel = default;
    [SerializeField] private FloatEventSO _SFXVolumeEvent = default;
    [SerializeField] private FloatEventSO _musicVolumeEvent = default;
    [SerializeField] private FloatEventSO _masterVolumeEvent = default;
    
    [Header("Audio control")]
    [SerializeField] private AudioMixer audioMixer = default;
    [Range(0f, 1f)]
    [SerializeField] private float _masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;
    
    private SoundEmitterVault _soundEmitterVault;
	private SoundEmitter _musicSoundEmitter;

	private void Awake()
	{
		_soundEmitterVault = new SoundEmitterVault();

		_pool.Prewarm(_initialSize);
		_pool.SetParent(this.transform);
	}

	private void OnEnable()
	{
		_SFXEventChannel.OnAudioPlayRequested += PlayAudioCue;
		_SFXEventChannel.OnAudioStopRequested += StopAudioCue;
		_SFXEventChannel.OnAudioFinishRequested += FinishAudioCue;

		_musicEventChannel.OnAudioPlayRequested += PlayMusicTrack;
		_musicEventChannel.OnAudioStopRequested += StopMusic;

		_masterVolumeEvent.OnEventRaised += ChangeMasterVolume;
		_musicVolumeEvent.OnEventRaised += ChangeMusicVolume;
		_SFXVolumeEvent.OnEventRaised += ChangeSFXVolume;

	}

	private void OnDestroy()
	{
		_SFXEventChannel.OnAudioPlayRequested -= PlayAudioCue;
		_SFXEventChannel.OnAudioStopRequested -= StopAudioCue;

		_SFXEventChannel.OnAudioFinishRequested -= FinishAudioCue;
		_musicEventChannel.OnAudioPlayRequested -= PlayMusicTrack;

		_musicVolumeEvent.OnEventRaised -= ChangeMusicVolume;
		_SFXVolumeEvent.OnEventRaised -= ChangeSFXVolume;
		_masterVolumeEvent.OnEventRaised -= ChangeMasterVolume;
	}

	void OnValidate()
	{
		if (Application.isPlaying)
		{
			SetGroupVolume("MasterVolume", _masterVolume);
			SetGroupVolume("MusicVolume", _musicVolume);
			SetGroupVolume("SFXVolume", _sfxVolume);
		}
	}
	void ChangeMasterVolume(float newVolume)
	{
		_masterVolume = newVolume;
		SetGroupVolume("MasterVolume", _masterVolume);
	}
	void ChangeMusicVolume(float newVolume)
	{
		_musicVolume = newVolume;
		SetGroupVolume("MusicVolume", _musicVolume);
	}
	void ChangeSFXVolume(float newVolume)
	{
		_sfxVolume = newVolume;
		SetGroupVolume("SFXVolume", _sfxVolume);
	}
	public void SetGroupVolume(string parameterName, float normalizedVolume)
	{
		bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
		if (!volumeSet)
			Debug.LogError("Параметр AudioMixer не найден");
	}

	public float GetGroupVolume(string parameterName)
	{
		if (audioMixer.GetFloat(parameterName, out float rawVolume))
		{
			return MixerValueToNormalized(rawVolume);
		}
		else
		{
			Debug.LogError("Параметр AudioMixer не найден");
			return 0f;
		}
	}

	private float MixerValueToNormalized(float mixerValue)
	{
		return 1f + (mixerValue / 80f);
	}
	private float NormalizedToMixerValue(float normalizedValue)
	{
		return (normalizedValue - 1f) * 80f;
	}

	private AudioKey PlayMusicTrack(AudioSO audioSrc, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
	{
		float fadeDuration = 2f;
		float startTime = 0f;

		if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
		{
			AudioClip songToPlay = audioSrc.GetClips()[0];
			if (_musicSoundEmitter.GetClip() == songToPlay)
				return AudioKey.Invalid;

			startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);
		}

		_musicSoundEmitter = _pool.Request();
		_musicSoundEmitter.FadeMusicIn(audioSrc.GetClips()[0], audioConfiguration, 1f, startTime);
		_musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

		return AudioKey.Invalid; 
	}

	private bool StopMusic(AudioKey key)
	{
		if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
		{
			_musicSoundEmitter.Stop();
			return true;
		}
		else
			return false;
	}

	public void TimelineInterruptsMusic()
	{
		StopMusic(AudioKey.Invalid);
	}

	public AudioKey PlayAudioCue(AudioSO audioSrc, AudioConfigurationSO settings, Vector3 position = default)
	{
		AudioClip[] clipsToPlay = audioSrc.GetClips();
		SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

		int nOfClips = clipsToPlay.Length;
		for (int i = 0; i < nOfClips; i++)
		{
			soundEmitterArray[i] = _pool.Request();
			if (soundEmitterArray[i] != null)
			{
				soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioSrc.looping, position);
				if (!audioSrc.looping)
					soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
			}
		}

		return _soundEmitterVault.Add(audioSrc, soundEmitterArray);
	}

	public bool FinishAudioCue(AudioKey audioKey)
	{
		bool isFound = _soundEmitterVault.Get(audioKey, out SoundEmitter[] soundEmitters);

		if (isFound)
		{
			for (int i = 0; i < soundEmitters.Length; i++)
			{
				soundEmitters[i].Finish();
				soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
			}
		}
		else
		{
			Debug.LogWarning("Запрос на завершение AudioSrc, но AudioSrc не найден.");
		}

		return isFound;
	}

	public bool StopAudioCue(AudioKey audioKey)
	{
		bool isFound = _soundEmitterVault.Get(audioKey, out SoundEmitter[] soundEmitters);

		if (isFound)
		{
			for (int i = 0; i < soundEmitters.Length; i++)
			{
				StopAndCleanEmitter(soundEmitters[i]);
			}

			_soundEmitterVault.Remove(audioKey);
		}

		return isFound;
	}

	private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
	{
		StopAndCleanEmitter(soundEmitter);
	}

	private void StopAndCleanEmitter(SoundEmitter soundEmitter)
	{
		if (!soundEmitter.IsLooping())
			soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

		soundEmitter.Stop();
		_pool.Return(soundEmitter);

	}

	private void StopMusicEmitter(SoundEmitter soundEmitter)
	{
		soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
		_pool.Return(soundEmitter);
	}
    
}
