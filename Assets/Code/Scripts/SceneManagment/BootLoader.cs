using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO _managersScene = default;
    [SerializeField] private GameSceneSO _menuToLoad = default;

    [SerializeField] private AssetReference _menuLoadChannel = default;

    private void Start()
    {
        _managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
         _menuLoadChannel.LoadAssetAsync<LoadEventSO>().Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<LoadEventSO> obj)
    {
        obj.Result.RaiseEvent(_menuToLoad, true);
    
        SceneManager.UnloadSceneAsync(0); //Boot is the only scene in BuildSettings, it has index 0
    }
    
}
