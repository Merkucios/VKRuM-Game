using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameObject Event")]
public class GameObjectEventSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;
	
    public void RaiseEvent(GameObject value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
