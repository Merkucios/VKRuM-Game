using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private GameObject _heroPrefab;
        
        
        public override void InstallBindings()
        {
            
        }
    }
}