using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Float Event")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;
	
    public void RaiseEvent(float value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
