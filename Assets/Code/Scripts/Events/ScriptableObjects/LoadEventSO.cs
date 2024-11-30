using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Load Event")]
public class LoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, bool> OnLoadingRequested;

    public void RaiseEvent(GameSceneSO locationToLoad, bool showLoadingScreen = false)
    {
        if (OnLoadingRequested != null)
        {
            OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen);
        }
        else
        {
            Debug.LogWarning("Событие загрузки сцены -> никто не ответил LoadEventChannelSO.cs");
        }
    }
}
