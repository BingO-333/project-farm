using UnityEngine;
using Zenject;

namespace Game
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] MoneyManager _moneyManager;

        public override void InstallBindings()
        {
            Container.Bind<MoneyManager>().FromInstance(_moneyManager).AsSingle();
        }
    }
}
