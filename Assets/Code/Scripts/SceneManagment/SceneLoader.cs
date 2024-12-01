using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO _gameplayScene = default;
	[SerializeField] private InputReader _inputReader = default;

	[Header("Loading Events")]
	[SerializeField] private LoadEventSO _loadLocation = default;
	[SerializeField] private LoadEventSO _loadMenu = default;

	[Header("Casting on")]
	[SerializeField] private BoolEventSO _toggleLoadingScreen = default;
	//Working up by the SpawnSystem
	[SerializeField] private VoidEventSO _onSceneReady = default; 

	private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
	private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

	//From scene loading requests get parameters 
	private GameSceneSO _sceneToLoad;
	private GameSceneSO _currentlyLoadedScene;
	private bool _showLoadingScreen;

	private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
	private bool _isLoading = false; 

	private void OnEnable()
	{
		_loadLocation.OnLoadingRequested += LoadLocation;
		_loadMenu.OnLoadingRequested += LoadMenu;
	}

	private void OnDisable()
	{
		_loadLocation.OnLoadingRequested -= LoadLocation;
		_loadMenu.OnLoadingRequested -= LoadMenu;
	}

	private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen)
	{
		if (_isLoading)
		{
			return;
		}

		_sceneToLoad = locationToLoad;
		_showLoadingScreen = showLoadingScreen;
		_isLoading = true;

		if (_gameplayManagerSceneInstance.Scene == null
			|| !_gameplayManagerSceneInstance.Scene.isLoaded)
		{
			_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			_gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
		}
		else
		{
			StartCoroutine(UnloadPreviousScene());
		}
	}

	private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

		StartCoroutine(UnloadPreviousScene());
	}

	private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen)
	{
		if (_isLoading)
		{
			return;
		}

		_sceneToLoad = menuToLoad;
		_showLoadingScreen = showLoadingScreen;
		_isLoading = true;

		if (_gameplayManagerSceneInstance.Scene != null
		    && _gameplayManagerSceneInstance.Scene.isLoaded)
		{
			Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
		}

		StartCoroutine(UnloadPreviousScene());
	}

	private IEnumerator UnloadPreviousScene()
	{
		_inputReader.DisableAllActionsMaps();
		
		yield return new WaitForSeconds(0.5f);

		if (_currentlyLoadedScene != null) 
		{
			if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
			{
				_currentlyLoadedScene.sceneReference.UnLoadScene();
			}
#if UNITY_EDITOR
			else
			{
				SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
			}
#endif
		}

		LoadNewScene();
	}

	private void LoadNewScene()
	{
		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(true);
		}

		_loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
		_loadingOperationHandle.Completed += OnNewSceneLoaded;
	}

	private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		_currentlyLoadedScene = _sceneToLoad;

		Scene s = obj.Result.Scene;
		SceneManager.SetActiveScene(s);
		LightProbes.TetrahedralizeAsync();

		_isLoading = false;

		if (_showLoadingScreen)
			_toggleLoadingScreen.RaiseEvent(false);

		StartGameplay();
	}

	private void StartGameplay()
	{
		_onSceneReady.RaiseEvent(); 
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("выход из игры!");
	}
}

