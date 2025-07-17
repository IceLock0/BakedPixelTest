using _Project.Scripts.Services.Log;
using _Project.Scripts.Services.SaveLoad;
using Zenject;

namespace _Project.Scripts.Installers.Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
        }

        private void BindServices()
        {
            Container.Bind<ILogService>().To<LogService>().AsSingle();
            
            Container.Bind<ISaveLoadService>().To<JsonSaveLoadService>().AsSingle();
        }
    }
}