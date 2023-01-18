using NM.Core.Interface;
using NM.Core.Services;
using Zenject;

namespace NM.Core.ZenjectInstallers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public GameDataService GameDataService;
        public AudioService AudioService;
        
        public override void InstallBindings()
        {
            BindGameDataService();
            BindAudioService();
        }

        private void BindAudioService()
        {
            Container.Bind<IAudioService>()
                .To<AudioService>()
                .FromComponentInNewPrefab(AudioService)
                .AsSingle()
                .NonLazy();
        }

        private void BindGameDataService()
        {
            Container.Bind<IGameDataService>()
                .To<GameDataService>()
                .FromComponentInNewPrefab(GameDataService)
                .AsSingle()
                .NonLazy();
        }
    }
}