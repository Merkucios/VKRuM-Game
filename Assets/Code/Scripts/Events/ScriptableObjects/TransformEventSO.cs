using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Transform Event")]
public class TransformEventSO : ScriptableObject
{
    public UnityAction<Transform> OnEventRaised;

    public void RaiseEvent(Transform value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
