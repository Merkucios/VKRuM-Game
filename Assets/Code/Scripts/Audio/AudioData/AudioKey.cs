using System;

public struct AudioKey
{
    public static AudioKey Invalid = new AudioKey(-1, null);

    internal int Value;
    internal AudioSO AudioCue;

    internal AudioKey(int value, AudioSO audioCue)
    {
        Value = value;
        AudioCue = audioCue;
    }

    public override bool Equals(Object obj)
    {
        return obj is AudioKey x && Value == x.Value && AudioCue == x.AudioCue;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode() ^ AudioCue.GetHashCode();
    }
    public static bool operator ==(AudioKey x, AudioKey y)
    {
        return x.Value == y.Value && x.AudioCue == y.AudioCue;
    }
    public static bool operator !=(AudioKey x, AudioKey y)
    {
        return !(x == y);
    }
}
