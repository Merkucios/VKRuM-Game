using System.Collections.Generic;

public class SoundEmitterVault
{
    private int _nextUniqueKey = 0;
    private List<AudioKey> _emittersKey;
    private List<SoundEmitter[]> _emittersList;

    public SoundEmitterVault()
    {
        _emittersKey = new List<AudioKey>();
        _emittersList = new List<SoundEmitter[]>();
    }

    public AudioKey GetKey(AudioSO audioSrc)
    {
        return new AudioKey(_nextUniqueKey++, audioSrc);
    }

    public void Add(AudioKey key, SoundEmitter[] emitter)
    {
        _emittersKey.Add(key);
        _emittersList.Add(emitter);
    }

    public AudioKey Add(AudioSO audioSrc, SoundEmitter[] emitter)
    {
        AudioKey emitterKey = GetKey(audioSrc);

        _emittersKey.Add(emitterKey);
        _emittersList.Add(emitter);

        return emitterKey;
    }

    public bool Get(AudioKey key, out SoundEmitter[] emitter)
    {
        int index = _emittersKey.FindIndex(x => x == key);

        if (index < 0)
        {
            emitter = null;
            return false;
        }

        emitter = _emittersList[index];
        return true;
    }

    public bool Remove(AudioKey key)
    {
        int index = _emittersKey.FindIndex(x => x == key);
        return RemoveAt(index);
    }

    private bool RemoveAt(int index)
    {
        if (index < 0)
        {
            return false;
        }

        _emittersKey.RemoveAt(index);
        _emittersList.RemoveAt(index);

        return true;
    }
}