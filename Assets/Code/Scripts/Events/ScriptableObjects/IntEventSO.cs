using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Int Event")]
public class IntEventSO : ScriptableObject
{
    public UnityAction<int> OnEventRaised;
	
    public void RaiseEvent(int value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
