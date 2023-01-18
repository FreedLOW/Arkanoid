using NM.Core.Interface;
using NM.Core.Services;
using NM.Game.Player;
using Zenject;

namespace NM.Core.ZenjectInstallers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public PlayerService PlayerService;
        public BallSpawnerService BallSpawnerService;
        public EventListenerService EventListenerService;
        public LevelDataService LevelDataService;
        public BlockSpawnerService BlockSpawnerService;

        public override void InstallBindings()
        {
            BindPlayerService();
            BindBallSpawnerService();
            BindEventListenerService();
            BindLevelDataService();
            BindBlockSpawnerService();
        }

        private void BindBlockSpawnerService()
        {
            Container.Bind<IBlockSpawnerService>()
                .To<BlockSpawnerService>()
                .FromComponentInNewPrefab(BlockSpawnerService)
                .AsSingle()
                .NonLazy();
        }

        private void BindLevelDataService()
        {
            Container.Bind<ILevelDataService>()
                .To<LevelDataService>()
                .FromComponentInNewPrefab(LevelDataService)
                .AsSingle()
                .NonLazy();
        }

        private void BindEventListenerService()
        {
            Container.Bind<IEventListenerService>()
                .To<EventListenerService>()
                .FromComponentInNewPrefab(EventListenerService)
                .AsSingle()
                .NonLazy();
        }

        private void BindPlayerService()
        {
            Container.Bind<IPlayerService>()
                .To<PlayerService>()
                .FromComponentInNewPrefab(PlayerService)
                .AsSingle()
                .NonLazy();
        }

        private void BindBallSpawnerService()
        {
            Container.Bind<IBallSpawnerService>()
                .To<BallSpawnerService>()
                .FromComponentInNewPrefab(BallSpawnerService)
                .AsSingle()
                .NonLazy();
        }
    }
}