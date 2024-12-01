using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameSceneSO _locationsToLoad;
    [SerializeField] private bool _showLoadScreen = default;
	
    [SerializeField] private LoadEventSO _loadLocation = default;

    [SerializeField] private VoidEventSO _onStartGame = default;
    
    private void Start()
    {
        _onStartGame.OnEventRaised += StartNewGame;
    }

    private void OnDestroy()
    {
        _onStartGame.OnEventRaised -= StartNewGame;
    }

    private void StartNewGame()
    {
        _loadLocation.RaiseEvent(_locationsToLoad, _showLoadScreen);
    }

}
