using UnityEngine;
using Zenject;

namespace Game
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] MoneyManager _moneyManager;
        [SerializeField] UIManager _uiManager;
        [SerializeField] Player _player;

        public override void InstallBindings()
        {
            Container.Bind<MoneyManager>().FromInstance(_moneyManager).AsSingle();
            Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
        }
    }
}
