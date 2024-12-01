using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/String Event")]
public class StringEventSO : ScriptableObject
{
    public UnityAction<string> OnEventRaised;
	
    public void RaiseEvent(string value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
