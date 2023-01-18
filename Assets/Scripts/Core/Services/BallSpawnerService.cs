using NM.Constant;
using NM.Core.Interface;
using NM.Game.Interface;
using UnityEngine;
using Zenject;

namespace NM.Core.Services
{
    public class BallSpawnerService : MonoBehaviour, IBallSpawnerService
    {
        [SerializeField] private GameObject ballPrefab;

        private ILevelDataService levelDataService;
        private IPlayerService playerService;
        private DiContainer container;

        [Inject]
        private void Construct(ILevelDataService levelDataService,
            IPlayerService playerService, DiContainer container)
        {
            this.levelDataService = levelDataService;
            this.playerService = playerService;
            this.container = container;
        }

        private void Start()
        {
            CreateBall();
        }

        public void CreateBall()
        {
            var position = playerService.BallPoint.position;
            var parent = playerService.PlayerBody.transform;
            var ball = container.InstantiatePrefab(ballPrefab);
            ball.transform.position = position;
            ball.transform.parent = parent;
            levelDataService.Balls.Add(ball);
        }

        public void CreateBall(Vector3 position)
        {
            var ballGameObject = container.InstantiatePrefab(ballPrefab);
            ballGameObject.transform.position = position;
            var ball = ballGameObject.GetComponent<IBallMovement>();
            ball.IsBallActive = true;
            ball.BallBody.bodyType = RigidbodyType2D.Dynamic;
            var randomDirection = new Vector2(Random.value > 0 ? 
                ConstantHolder.BallForceX : -ConstantHolder.BallForceX, ConstantHolder.BallForceY);
            ball.BallBody.AddForce(randomDirection);
            levelDataService.Balls.Add(ballGameObject);
        }
    }
}