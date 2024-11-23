using Zenject;
using UnityEngine;

namespace Code.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        public GameInputLoader gameInputLoader; // Ссылка на GameInputLoader
        public InputReader inputReader;         // Ссылка на ScriptableObject InputReader

        public override void InstallBindings()
        {
            // Убедитесь, что inputReader привязан
            if (inputReader == null)
            {
                Debug.LogError("InputReader is not set in BootstrapInstaller!");
                return;
            }

            Container.Bind<InputReader>().FromScriptableObject(inputReader).AsSingle();

            // Убедитесь, что gameInputLoader правильно настроен
            if (gameInputLoader == null || gameInputLoader.gameInput == null)
            {
                Debug.LogError("GameInputLoader or GameInput is not set in BootstrapInstaller!");
                return;
            }

            Container.Bind<GameInput>().FromInstance(gameInputLoader.gameInput).AsSingle();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsTransient();
        }
    }
}