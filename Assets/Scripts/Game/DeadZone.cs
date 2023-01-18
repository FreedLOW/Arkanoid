using Core.Enums;
using NM.Core.Interface;
using NM.Game.Interface;
using NM.Game.Level;
using UnityEngine;
using Zenject;

namespace NM.Game
{
    public class DeadZone : MonoBehaviour
    {
        private const int Damage = 1;

        private IBallSpawnerService ballSpawnerService;
        private IEventListenerService eventListenerService;
        private ILevelDataService levelDataService;
        private IAudioService audioService;
        
        [Inject]
        private void Construct(IBallSpawnerService ballSpawnerService, IEventListenerService eventListenerService,
            ILevelDataService levelDataService, IAudioService audioService)
        {
            this.ballSpawnerService = ballSpawnerService;
            this.eventListenerService = eventListenerService;
            this.levelDataService = levelDataService;
            this.audioService = audioService;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IBallMovement ballMovement))
            {
                ballMovement.DestroyBall();
                if (levelDataService.Balls.Count >= 1)
                {
                    audioService.PlayOneShotAudioSound(AudioKey.BallLoss);
                    return;
                }
                audioService.PlayOneShotAudioSound(AudioKey.BallLoss);
                eventListenerService.InvokeOnPlayerHealthChange(Damage);
                var isGameOver = eventListenerService.InvokeIsGameOver();
                if (isGameOver)
                {
                    audioService.PlayOneShotAudioSound(AudioKey.GameOver);
                    eventListenerService.InvokeGameOver();
                    return;
                }
                ballSpawnerService.CreateBall();
            }

            if (col.TryGetComponent(out Bonus bonus))
            {
                Destroy(bonus.gameObject);
            }
        }
    }
}