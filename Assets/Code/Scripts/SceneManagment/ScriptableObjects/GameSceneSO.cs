using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameSceneSO : ScriptableObject
{
    public GameSceneType sceneType;
    public AssetReference sceneReference;
    
    public enum GameSceneType
    {
        // Gameplay Scenes
        Location, 
        Menu, 

        //Special scenes
        Boot,
        Gameplay,
        PersistentManagers,

    }
}
