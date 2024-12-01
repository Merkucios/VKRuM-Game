using UnityEngine;
using UnityEngine.Events;

// Create only one manager in game
[CreateAssetMenu(menuName = "Managers/Pins Manager")]
public class PinManagerSO : ScriptableObject
{
    [SerializeField] private IntEventSO OnPinsUpdatedEvent;
    
    private int _totalPins = 0;
    private int _knockedDownPins = 0;

    public void RegisterPin()
    {
        _totalPins++;
        UpdatePins();
    }

    public void KnockDownPin()
    {
        _knockedDownPins++;
        UpdatePins();
    }

    private void UpdatePins()
    {
        OnPinsUpdatedEvent.RaiseEvent(_knockedDownPins);
    }

    public void ResetPins()
    {
        _totalPins = 0;
        _knockedDownPins = 0;
        UpdatePins();
    }

    public int GetTotalPins() => _totalPins;
    public int GetKnockedDownPins() => _knockedDownPins;
    
}