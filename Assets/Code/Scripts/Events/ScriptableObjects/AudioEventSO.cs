using UnityEngine;

[CreateAssetMenu(menuName = "Events/Audio Event")]
public class AudioEventSO : ScriptableObject
{
    public AudioPlayAction OnAudioPlayRequested;
    public AudioStopAction OnAudioStopRequested;
    public AudioFinishAction OnAudioFinishRequested;

    public AudioKey RaisePlayEvent(AudioSO audioSrc, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
    {
        AudioKey audioKey = AudioKey.Invalid;

        if (OnAudioPlayRequested != null)
        {
            audioKey = OnAudioPlayRequested.Invoke(audioSrc, audioConfiguration, positionInSpace);
        }
        else
        {
            Debug.LogWarning("Был запрошен запуск AudioSrc для " + audioSrc.name + ", но никто не обработал событие. " +
                             "Проверьте, почему AudioManager не загружен, " +
                             "и убедитесь, что он подписан на этот AudioSrc Event канал.");
        }

        return audioKey;
    }

    public bool RaiseStopEvent(AudioKey audioKey)
    {
        bool requestSucceed = false;

        if (OnAudioStopRequested != null)
        {
            requestSucceed = OnAudioStopRequested.Invoke(audioKey);
        }
        else
        {
            Debug.LogWarning("Был запрошен останов AudioSrc, но никто не обработал событие. " +
                             "Проверьте, почему AudioManager не загружен, " +
                             "и убедитесь, что он подписан на этот AudioSrc Event канал.");
        }

        return requestSucceed;
    }

    public bool RaiseFinishEvent(AudioKey audioKey)
    {
        bool requestSucceed = false;

        if (OnAudioStopRequested != null)
        {
            requestSucceed = OnAudioFinishRequested.Invoke(audioKey);
        }
        else
        {
            Debug.LogWarning("Было запрошено завершение AudioSrc, но никто не обработал событие. " +
                             "Проверьте, почему AudioManager не загружен, " +
                             "и убедитесь, что он подписан на этот AudioSrc Event канал.");
        }

        return requestSucceed;
    }
}

public delegate AudioKey AudioPlayAction(AudioSO audioSrc, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace);
public delegate bool AudioStopAction(AudioKey emitterKey);
public delegate bool AudioFinishAction(AudioKey emitterKey);
